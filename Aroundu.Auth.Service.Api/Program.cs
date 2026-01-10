using Aroundu.Auth.Service.Infrastructure.EFCore;
using Aroundu.SharedKernel.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Aroundu.Auth.Service.Api;

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
            mt.AddEntityFrameworkOutbox<AuthDbContext>(o =>
            {
                o.UseSqlServer();
                o.UseBusOutbox();

                // Uncomment the following lines to disable the delivery service if needed (for testing)
                //o.UseBusOutbox(outboxConfig =>
                //    {
                //        outboxConfig.DisableDeliveryService();
                //    });

                o.DuplicateDetectionWindow = TimeSpan.FromMinutes(30);
                o.QueryDelay = TimeSpan.FromSeconds(10);
            });

            mt.UsingRabbitMq((context, conf) =>
            {
                conf.Host(builder.Configuration.GetConnectionString("messaging"));
                conf.ConfigureEndpoints(context);
            });
        });
        builder.Services.Scan(scan => scan
            .FromAssemblies(
                typeof(Aroundu.Auth.Service.Infrastructure.Scrutor.AssemblyMarker).Assembly,
                typeof(Aroundu.Auth.Service.Application.Scrutor.AssemblyMarker).Assembly
            )
            .AddClasses(classes => classes.AssignableTo<IDependency>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        builder.AddSqlServerDbContext<AuthDbContext>("auth-db");
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Aroundu.Auth.Service.Application.Scrutor.AssemblyMarker).Assembly);
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
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
