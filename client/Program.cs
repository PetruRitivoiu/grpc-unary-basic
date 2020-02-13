using Greet;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        const string target = "127.0.0.1:5001";

        static void Main(string[] args)
        {
            var channel = new Channel(target, ChannelCredentials.Insecure);

            channel.ConnectAsync().ContinueWith(task =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Client connected successfully");
                }
            });

            var client = new GreetingService.GreetingServiceClient(channel);

            var greeting = new Greeting()
            {
                FirstName = "Clement",
                LastName = "Jean"
            };

            var request = new GreetingRequest() { Greeting = greeting };
            var response = client.Greet(request);

            Console.WriteLine(response.Result);

            channel.ShutdownAsync().Wait();

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
