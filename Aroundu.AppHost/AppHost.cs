var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("messaging");

var sql = builder.AddSqlServer("sql-server")
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

var eventsDb = sql.AddDatabase("events-db");

var eventsService = builder.AddProject<Projects.Aroundu_Events_Service_Api>("aroundu-events-service-api")
    .WithReference(messaging)
    .WithReference(eventsDb);

var authService = builder.AddProject<Projects.Aroundu_Auth_Service_Api>("aroundu-auth-service-api")
    .WithReference(messaging);

var gateway = builder.AddProject<Projects.Aroundu_Api_Gateway>("aroundu-api-gateway")
    .WithReference(messaging)
    .WithReference(authService)
    .WithReference(eventsService);

builder.AddNpmApp("angular-web", "../Aroundu.Web")
    .WithReference(gateway)
    .WithHttpsEndpoint(targetPort: 4200, name: "https")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
