using Greet;
using Grpc.Core;
using Sqrt;
using System;
using System.Collections.Generic;
using System.IO;

namespace server
{
    class Program
    {
        const int port = 5001;

        static void Main(string[] args)
        {
            var serverCert = File.ReadAllText("ssl/server.crt");
            var serverKey = File.ReadAllText("ssl/server.key");
            var caCert = File.ReadAllText("ssl/ca.crt");

            var keyPair = new KeyCertificatePair(serverCert, serverKey);

            var credentials = new SslServerCredentials(new List<KeyCertificatePair>() { keyPair }, caCert, true);

            Server server = null;

            try
            {
                server = new Server()
                {
                    Services = { GreetingService.BindService(new GreetingServiceImpl()),
                                 SqrtService.BindService(new SqrtServiceImpl()) },
                    Ports = { new ServerPort("localhost", port, credentials) }
                };

                server.Start();
                Console.WriteLine($"Server listening on port : {port}");
                Console.WriteLine("Press any key ...");
                Console.ReadKey();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"The server failed to start: {ex}");
                throw;
            }
            finally
            {
                if (server != null)
                {
                    server.ShutdownAsync().Wait();
                }
            }
        }
    }
}
