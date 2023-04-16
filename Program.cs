using System.Diagnostics;
using grpc_open_tel.Services;
using Grpc.Core;
using Grpc.Core.Interceptors;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Status = OpenTelemetry.Trace.Status;
using StatusCode = OpenTelemetry.Trace.StatusCode;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddOpenTelemetry().WithTracing(builder => builder.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddService("grpc-server"))
    .AddSource("grpc-server")
    .AddAspNetCoreInstrumentation()
    .AddGrpcClientInstrumentation()
    .AddJaegerExporter());

builder.Services.AddGrpc(options => { options.Interceptors.Add<ErrorDescriptionInterceptor>();});
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcReflectionService();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

public class ErrorDescriptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // This is a workaround for https://github.com/grpc/grpc-dotnet/issues/1407
            Activity.Current?.RecordException(ex);
            throw;
        }
    }
}