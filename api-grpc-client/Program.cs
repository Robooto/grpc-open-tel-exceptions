using grpc_open_tel;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenTelemetry().WithTracing(builder => builder.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddService("api-grpc-client"))
    .AddSource("api-grpc-client")
    .AddAspNetCoreInstrumentation()
    .AddHttpClientInstrumentation()
    .AddGrpcClientInstrumentation()
    .AddJaegerExporter());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpcClient<Greeter.GreeterClient>(options =>
{
    options.Address = new Uri("http://grpc-open-tel");
}).ConfigurePrimaryHttpMessageHandler(GetInsecureHandler);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static HttpClientHandler GetInsecureHandler()
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
}