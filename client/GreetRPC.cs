using System;
using System.IO;
using System.Threading.Tasks;
using Greet;
using Grpc.Core;

namespace client
{
    public static class GreetRPC
    {
        public static async Task<GreetingResponse> Execute(string host, int port, GreetingRequest greetingRequest)
        {
            var clientCert = File.ReadAllText("ssl/client.crt");
            var clientKey = File.ReadAllText("ssl/client.key");
            var caCrt = File.ReadAllText("ssl/ca.crt");

            var channelCredentials = new SslCredentials(caCrt, new KeyCertificatePair(clientCert, clientKey));

            Channel channel = null;
            GreetingResponse response;

            try
            {
                channel = new Channel(host, port, channelCredentials);
                await channel.ConnectAsync();

                var client = new GreetingService.GreetingServiceClient(channel);
                response = await client.GreetAsync(greetingRequest, deadline: DateTime.UtcNow.AddMilliseconds(5000));

            }
            catch (Exception)
            {
                // log
                throw;
            }
            finally
            {
                if (channel != null)
                {
                    await channel.ShutdownAsync();
                }
            }

            return response;
        }
    }
}
