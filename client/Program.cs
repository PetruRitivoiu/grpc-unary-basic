using Greet;
using Sqrt;
using System;

namespace client
{
    class Program
    {
        const string target = "127.0.0.1:5001"; // for insecure channels
        const string host = "localhost"; // for secure channels
        const int port = 5001; // for secure channels

        static void Main(string[] args)
        {
            string exit = "^C";
            string input = string.Empty;

            while (input != exit)
            {
                Console.WriteLine("Press 1 for Greeting");
                Console.WriteLine("Press 2 for SQRT");
                Console.WriteLine("Press ^C for exit");
                input = Console.ReadLine();

                if (input == "1")
                {
                    handleGreeting();
                }
                if (input == "2")
                {
                    handleSqrt();
                }

                Console.WriteLine();
            }

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }

        public static void handleGreeting()
        {
            var greeting = new Greeting()
            {
                FirstName = "Petru",
                LastName = "Ritivoiu"
            };

            var greetingRequest = new GreetingRequest() { Greeting = greeting };

            var greetingResponse = GreetRPC.Execute(host, port, greetingRequest).Result;

            Console.WriteLine($"Result: {greetingResponse.Result}");
        }

        public static void handleSqrt()
        {
            Console.WriteLine("Type a number");
            var numberStr = Console.ReadLine();
            int.TryParse(numberStr, out var number);

            var sqrtRequest = new SqrtRequest()
            {
                Number = number
            };

            var sqrtResponse = SqrtRPC.Execute(host, port, sqrtRequest).Result;

            if (sqrtResponse != null)
            {
                Console.WriteLine($"Result: {sqrtResponse.SquareRoot}");
            }

        }
    }
}
