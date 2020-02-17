using System;
using System.IO;
using System.Threading.Tasks;
using Grpc.Core;
using Sqrt;

namespace client
{
    public class SqrtRPC
    {
        public static async Task<SqrtResponse> Execute(string host, int port, SqrtRequest sqrtRequest)
        {
            var clientCert = File.ReadAllText("ssl/client.crt");
            var clientKey = File.ReadAllText("ssl/client.key");
            var caCrt = File.ReadAllText("ssl/ca.crt");

            var channelCredentials = new SslCredentials(caCrt, new KeyCertificatePair(clientCert, clientKey));

            Channel channel = null;
            SqrtResponse response = null;

            try
            {
                channel = new Channel(host, port, channelCredentials);

                await channel.ConnectAsync();

                var client = new SqrtService.SqrtServiceClient(channel);

                response = await client.SqrtAsync(sqrtRequest, deadline: DateTime.UtcNow.AddMilliseconds(5000));
            }
            catch (RpcException ex) when (ex.Status.StatusCode == StatusCode.InvalidArgument)
            {
                Console.WriteLine($"RPC Exception: {ex}");
                Console.WriteLine($"Status Code: {ex.Status.StatusCode}");
                Console.WriteLine($"Status Detail: {ex.Status.Detail}");
            }
            catch(Exception)
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
