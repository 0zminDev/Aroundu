var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("messaging");

builder.AddProject<Projects.Aroundu_Api_Gateway>("aroundu-api-gateway")
    .WithReference(messaging);

builder.AddProject<Projects.Aroundu_Events_Service_Api>("aroundu-events-service-api")
    .WithReference(messaging);

builder.AddProject<Projects.Aroundu_Auth_Service_Api>("aroundu-auth-service-api")
    .WithReference(messaging);

builder.Build().Run();
