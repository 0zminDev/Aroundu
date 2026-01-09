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

builder.AddProject<Projects.Aroundu_Api_Gateway>("aroundu-api-gateway")
    .WithReference(messaging)
    .WithReference(authService)
    .WithReference(eventsService);

builder.Build().Run();
