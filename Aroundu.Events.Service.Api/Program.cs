using Aroundu.Events.Service.Infrastructure.EFCore;
using Aroundu.SharedKernel.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Aroundu.Events.Service.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMassTransit(mt =>
        {
            mt.UsingRabbitMq((context, conf) =>
            {
                conf.Host(builder.Configuration.GetConnectionString("messaging"));
                conf.ConfigureEndpoints(context);
            });
        });
        builder.Services.Scan(scan => scan
            .FromAssemblies(
                typeof(Aroundu.Events.Service.Infrastructure.Scrutor.AssemblyMarker).Assembly,
                typeof(Aroundu.Events.Service.Application.Scrutor.AssemblyMarker).Assembly
            )
            .AddClasses(classes => classes.AssignableTo<IDependency>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        builder.AddSqlServerDbContext<EventsDbContext>("events-db");
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Aroundu.Events.Service.Application.Scrutor.AssemblyMarker).Assembly);
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<EventsDbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration Error: {ex.Message}");
            }
        }

        app.MapDefaultEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
