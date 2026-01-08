using MassTransit;

namespace Aroundu.Api.Gateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
        builder.Services.AddMassTransit(mt =>
        {
            mt.UsingRabbitMq((context, conf) =>
            {
                conf.Host(builder.Configuration.GetConnectionString("messaging"));
                conf.ConfigureEndpoints(context);
            });
        });

        var app = builder.Build();

        app.MapDefaultEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapReverseProxy();
        app.MapControllers();

        app.Run();
    }
}
