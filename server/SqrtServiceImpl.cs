using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Sqrt;
using static Sqrt.SqrtService;

namespace server
{
    public class SqrtServiceImpl : SqrtServiceBase
    {
        public override Task<SqrtResponse> Sqrt(SqrtRequest request, ServerCallContext context)
        {
            if (request.Number >= 0)
            {
                var res = new SqrtResponse() { SquareRoot = Math.Sqrt(request.Number) };
                return Task.FromResult(res);
            } else
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    $"number {request.Number} has to be greater or equal than 0"));
            }
        }
    }
}
