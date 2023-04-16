using Grpc.Core;
using grpc_open_tel;

namespace grpc_open_tel.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override Task<HelloReply> SayHelloAgain(HelloRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.Internal, "DB connection failed"), "DB connection failed");
    }
}