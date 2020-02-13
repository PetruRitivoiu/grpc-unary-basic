using Greet;
using Grpc.Core;
using System;
using System.IO;

namespace server
{
    class Program
    {
        const int port = 5001;

        static void Main(string[] args)
        {
            Server server = null;

            try
            {
                server = new Server()
                {
                    Services = { GreetingService.BindService(new GreetingServiceImpl()) },
                    Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
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
