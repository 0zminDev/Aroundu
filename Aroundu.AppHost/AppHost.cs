var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("messaging");

var password = builder.AddParameter("sql-password", secret: true);

var sql = builder
    .AddSqlServer("sql-server", password)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEnvironment("ACCEPT_EULA", "Y");

sql.WithEndpoint(
    "tcp",
    endpoint =>
    {
        endpoint.Port = 14333;
        endpoint.IsProxied = false;
    }
);

var eventsDb = sql.AddDatabase("events-db");

var eventsService = builder
    .AddProject<Projects.Aroundu_Events_Service_Api>("aroundu-events-service-api")
    .WithReference(messaging)
    .WithEnvironment(
        "ConnectionStrings__events-db",
        ReferenceExpression.Create($"{eventsDb};Encrypt=False;TrustServerCertificate=True")
    )
    .WaitFor(eventsDb);

var authService = builder
    .AddProject<Projects.Aroundu_Auth_Service_Api>("aroundu-auth-service-api")
    .WithReference(messaging);

var gateway = builder
    .AddProject<Projects.Aroundu_Api_Gateway>("aroundu-api-gateway")
    .WithReference(messaging)
    .WithReference(authService)
    .WithReference(eventsService);

builder
    .AddNpmApp("angular-web", "../Aroundu.Web")
    .WithReference(gateway)
    .WithHttpsEndpoint(port: 4200, targetPort: 4200, name: "https", isProxied: false)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
