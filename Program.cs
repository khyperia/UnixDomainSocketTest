using System;
using System.IO;
using System.Net.Sockets;
using Microsoft.CodeAnalysis.CompilerServer;

namespace UnixDomainSocketTest
{
    public static class Program
    {
        private const string PipeName = "test_pipe";

        private static void RunServer()
        {
            using (var server = UnixDomainSocket.CreateServer(PipeName))
            {
                while (true)
                {
                    using (var accepted = server.WaitOne())
                    using (var stream = new NetworkStream(accepted))
                    using (var reader = new StreamReader(stream))
                    using (var writer = new StreamWriter(stream))
                    {
                        Console.WriteLine("Server accepted");
                        writer.WriteLine("Server -> Client");
                        writer.Flush();
                        Console.WriteLine("Server written");
                        Console.WriteLine(reader.ReadLine());
                        Console.WriteLine("Server done");
                    }
                }
            }
        }

        private static void RunClient()
        {
            using (var client = UnixDomainSocket.CreateClient(PipeName))
            using (var stream = new NetworkStream(client))
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                Console.WriteLine("Client connected");
                writer.WriteLine("Server -> Client");
                writer.Flush();
                Console.WriteLine("Client written");
                Console.WriteLine(reader.ReadLine());
                Console.WriteLine("Client done");
            }
        }

        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid usage");
                return;
            }
            Console.WriteLine("Go!");
            switch (args[0])
            {
                case "server":
                    RunServer();
                    break;
                case "client":
                    RunClient();
                    break;
            }
        }
    }
}
