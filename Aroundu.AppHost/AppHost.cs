using Aroundu.AppHost.Helpers;

var builder = DistributedApplication.CreateBuilder(args);
var systemBuilder = new SystemBuilder(builder);

builder.AddProject<Projects.Aroundu_ApiGateway>("aroundu_api_gateway");

builder.Build().Run();
