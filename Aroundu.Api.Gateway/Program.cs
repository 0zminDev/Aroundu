using MassTransit;

namespace Aroundu.Api.Gateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Services.AddControllers(); 
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
            .AddServiceDiscoveryDestinationResolver();

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
            app.UseSwagger(); 
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API");
                c.SwaggerEndpoint("/swagger-events/swagger/v1/swagger.json", "Events Service");
                c.SwaggerEndpoint("/swagger-auth/swagger/v1/swagger.json", "Auth Service");
            });
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAngular");
        app.UseAuthorization();
        app.MapReverseProxy();
        app.MapControllers();

        app.Run();
    }
}
