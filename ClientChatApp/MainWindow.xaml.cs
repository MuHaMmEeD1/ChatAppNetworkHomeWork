using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatAppNetworkHomeWork.Models;
using System.Text.Json;
using System.Xml.Linq;

namespace ClientChatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        IPEndPoint remoutEP;

        TcpClient tcpClient;

        BinaryReader binaryReader;
        BinaryWriter binaryWriter;
        string Name = "";
        bool qeydiyyat = false;

        public MainWindow()
        {
            InitializeComponent();

            remoutEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
            tcpClient = new TcpClient();
           

            tcpClient.Connect(remoutEP);



            binaryWriter = new BinaryWriter(tcpClient.GetStream());
            binaryReader = new BinaryReader(tcpClient.GetStream());

            _ = Task.Run(() => {
            
                while(true)
                {
               


                    string Messeg = binaryReader.ReadString();

                    User user = new User();
                    user = JsonSerializer.Deserialize<User>(Messeg);
                    Dispatcher.Invoke(() => { ThisMessegSTP.Children.Add(new Label() { Content = user, Height = 30 }); });
                    
                }
           
            
            });



        }


        async Task EnterMessegAsync()
        {
            if (NameTextBox.Text != Name && NameTextBox.Text.Length != 0)
            {
                var localEndPoint = (IPEndPoint)tcpClient.Client.LocalEndPoint;

                User user = new User()
                {
                    Name = NameTextBox.Text,
                    Messeg = MessegTextBox.Text,
                    Port = localEndPoint.Port,
                    Gonderen = Name

                };
                binaryWriter.Write(JsonSerializer.Serialize(user));
                user.Name = Name;

                YorMessegSTP.Children.Add(new Label() { Content = user, Height = 30 });
            }
            else { MessageBox.Show("Ad Duzgun Qeyd Olunmayib"); }
        }


        async Task QeydiyyatMetedAsync()
        {
            var localEndPoint = (IPEndPoint)tcpClient.Client.LocalEndPoint;

            Name = NameTextBox.Text;

            User user = new User()
            {
                Name = NameTextBox.Text,
                Messeg = MessegTextBox.Text,
                Port = localEndPoint.Port

            };

            binaryWriter.Write(JsonSerializer.Serialize(user).ToString());
            
        }


        private void GonderButton_Click(object sender, RoutedEventArgs e)
        {
            if (qeydiyyat)
            {
                EnterMessegAsync();
            }
            else { MessageBox.Show("Siz Qeydiyyatdan Kecmemisiz"); }
        }

        private async void GeqdiyyatButton_Click(object sender, RoutedEventArgs e)
        {
            if (!qeydiyyat)
            {
                await QeydiyyatMetedAsync();
                
                qeydiyyat = true;
            }
            else { MessageBox.Show("Siz artiq qeydiyytdan kecmisiz"); }
        }
    }
}