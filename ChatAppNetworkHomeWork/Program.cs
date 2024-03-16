

using ChatAppNetworkHomeWork.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
TcpListener tcpListener = new TcpListener(endPoint);
List<User> users = new List<User>();
Dictionary<string, TcpClient> keyValues = new Dictionary<string, TcpClient>();

tcpListener.Start();


while (true)
{
  
    var client = tcpListener.AcceptTcpClient();

    Console.WriteLine("Elaqe Quruldu "+ client.Client.LocalEndPoint);
    Console.WriteLine("Elaqe Quruldu "+ client.Client.RemoteEndPoint);

    _ = Task.Run(()=>{

        TcpClient tcpClient = client;
        BinaryReader binaryReader = new BinaryReader(tcpClient.GetStream());
        
        User user = new User();

        user = JsonSerializer.Deserialize<User>(binaryReader.ReadString());
        users.Add(user);
        keyValues.Add(user.Name, tcpClient);
        Console.WriteLine("PORT "+ user.Port);

        

        Console.WriteLine($"{user.Name} qeydiyatdan kecdi");


        while (tcpClient.Connected)
        {
            User userGedecek = new User();
            userGedecek = JsonSerializer.Deserialize<User>(binaryReader.ReadString());

            Console.WriteLine(userGedecek.Name + " geldi mesaj");

            foreach (var us in users)
            {
                if (us.Name == userGedecek.Name) {


                    foreach (var key in keyValues)
                    {

                        if (key.Key == userGedecek.Name)
                        {

                            var Br = new BinaryWriter(key.Value.GetStream());
                            userGedecek.Name = userGedecek.Gonderen;
                            Br.Write(JsonSerializer.Serialize(userGedecek));
                            break;
                        }
                    }
                    
                    Console.WriteLine("Mesaj gonderildi");

                    break;
                }
            }


           

        }

    });

}

























