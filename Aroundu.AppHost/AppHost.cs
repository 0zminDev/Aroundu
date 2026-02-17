using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var isTesting = builder.Environment.IsEnvironment("Testing");
var isPerformanceTesting = builder.Environment.IsEnvironment("PerformanceTesting");

var messaging = builder
    .AddRabbitMQ("messaging")
    .WithManagementPlugin()
    .WithContainerRuntimeArgs("--memory=256m");

var password = builder.AddParameter("sql-password", secret: true);

var sql = builder
    .AddSqlServer("sql-server", password)
    .WithImage("mssql/server", "2022-latest")
    .WithContainerRuntimeArgs("--memory=2g", "--cpus=1.0", "--userns=keep-id")
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithLifetime(isTesting ? ContainerLifetime.Session : ContainerLifetime.Persistent);

if (!isTesting)
{
    sql.WithDataVolume(isReadOnly: false);

    sql.WithEndpoint(
        "tcp",
        endpoint =>
        {
            endpoint.Port = 14333;
            endpoint.IsProxied = false;
        }
    );
}

var eventsDb = sql.AddDatabase("events-db");
var authDb = sql.AddDatabase("auth-db");

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
    .WithReference(messaging)
    .WithReference(authDb);

var gateway = builder
    .AddProject<Projects.Aroundu_Api_Gateway>("aroundu-api-gateway")
    .WithReference(messaging)
    .WithReference(authService)
    .WithReference(eventsService);

if (!isTesting)
{
    builder
        .AddExecutable("angular-web", "npm", "../Aroundu.Web", "run", "start")
        .WithReference(gateway)
        .WithHttpsEndpoint(port: 4200, targetPort: 4200, name: "https", isProxied: false)
        .WithExternalHttpEndpoints()
        .PublishAsDockerFile();
}

var app = builder.Build();

try
{
    if (!isTesting)
    {
        await app.RunAsync();
    }
}
catch (OperationCanceledException ex)
{
    if (!isTesting)
    {
        Console.WriteLine($"Service terminated unexpectedly: {ex.Message}");
    }
}
catch (Exception ex)
{
    if (!ex.Message.Contains("canceled", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine($"Service terminated unexpectedly: {ex.Message}");
    }
    throw;
}
