using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaWebApi.Actors;

namespace AkkaWebApi
{
    class Program
    {
        public static ActorSystem System { get; private set; }

        static void Main(string[] args)
        {
            System = ActorSystem.Create("server");
            var api = System.ActorOf<ServerActor>("api");
            api.Tell("start");

            Console.ReadLine();
        }
    }
}
