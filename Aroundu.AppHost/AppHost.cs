using Aroundu.AppHost.Model.Enums;
using Aroundu.AppHost.Helpers;

var builder = DistributedApplication.CreateBuilder(args);

var systemBuilder = new SystemBuilder(builder);


builder.Build().Run();
