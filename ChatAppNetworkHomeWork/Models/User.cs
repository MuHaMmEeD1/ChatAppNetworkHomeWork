using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatAppNetworkHomeWork.Models
{
    public  class User
    {
        public string Name { get; set; }
        public string Messeg { get; set; }
        public int Port { get; set; }
        public string Gonderen { get; set; }

        public override string ToString()
        {
            return $"{Name} -> {{ {Messeg} }} [{DateTime.Now}]";
        }
    }
}
