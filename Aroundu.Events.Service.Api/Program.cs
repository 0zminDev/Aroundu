using Aroundu.Events.Service.Infrastructure.EFCore;
using Aroundu.Events.Service.Infrastructure.Infrastructure.ExeptionHandler;
using Aroundu.Events.Service.Infrastructure.Infrastructure.MassTransit;
using Aroundu.Events.Service.Infrastructure.Infrastructure.ValidationBehavior;
using Aroundu.SharedKernel.Interfaces;
using Aroundu.SharedKernel.Interfaces.Events;
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
            var appAssembly =
                typeof(Aroundu.Events.Service.Application.Scrutor.AssemblyMarker).Assembly;

            var handlerTypes = appAssembly
                .GetTypes()
                .Where(t =>
                    t.GetInterfaces()
                        .Any(i =>
                            i.IsGenericType
                            && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)
                        )
                )
                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                var eventType = handlerType
                    .GetInterfaces()
                    .First(i =>
                        i.IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>)
                    )
                    .GetGenericArguments()[0];

                mt.AddConsumer(
                    typeof(MassTransitIntegrationEventHandlerAdapter<>).MakeGenericType(eventType)
                );
            }

            mt.AddEntityFrameworkOutbox<EventsDbContext>(o =>
            {
                o.UseSqlServer();
                o.UseBusOutbox(c =>
                {
                    // Uncomment the following line to disable the delivery service if needed (for testing)
                    //c.DisableDeliveryService();
                });
                o.UseBusOutbox();

                o.DuplicateDetectionWindow = TimeSpan.FromMinutes(5);
                o.QueryDelay = TimeSpan.FromSeconds(30);
            });

            mt.SetKebabCaseEndpointNameFormatter();

            mt.UsingRabbitMq(
                (context, conf) =>
                {
                    conf.Host(builder.Configuration.GetConnectionString("messaging"));
                    conf.ConfigureEndpoints(context);
                }
            );
        });
        builder.Services.Scan(scan =>
            scan.FromAssemblies(
                    typeof(Aroundu.Events.Service.Infrastructure.Scrutor.AssemblyMarker).Assembly,
                    typeof(Aroundu.Events.Service.Application.Scrutor.AssemblyMarker).Assembly
                )
                .AddClasses(classes => classes.AssignableTo<IDependency>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        builder.AddSqlServerDbContext<EventsDbContext>(
            "events-db",
            configureDbContextOptions: options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("events-db"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null
                        );
                    }
                );
            }
        );
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(Aroundu.Events.Service.Application.Scrutor.AssemblyMarker).Assembly
            );

            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        builder.Services.Scan(scan =>
            scan.FromAssemblies(
                    typeof(Aroundu.Events.Service.Infrastructure.Scrutor.AssemblyMarker).Assembly,
                    typeof(Aroundu.Events.Service.Application.Scrutor.AssemblyMarker).Assembly
                )
                .AddClasses(classes => classes.AssignableTo<IDependency>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<EventsDbContext>();
                context.Database.Migrate();
                try
                {
                    context.Database.ExecuteSqlRaw(
                        "ALTER DATABASE [events-db] SET READ_COMMITTED_SNAPSHOT ON WITH ROLLBACK IMMEDIATE"
                    );
                }
                catch (Exception)
                {
                    // NOTE: Ignore if already enabled
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration Error: {ex.Message}");
            }
        }

        app.MapDefaultEndpoints();
        app.UseExceptionHandler();

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
