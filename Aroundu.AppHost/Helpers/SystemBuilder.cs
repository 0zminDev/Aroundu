using Aroundu.AppHost.Model.Enums;

namespace Aroundu.AppHost.Helpers;

public class SystemBuilder
{
    private readonly IDistributedApplicationBuilder DistributedApplicationBuilder;

    public SystemEnvironment SystemEnvironment { get; init; }

    public SystemBuilder(IDistributedApplicationBuilder builder)
    {
        this.DistributedApplicationBuilder = builder;

        this.SystemEnvironment = this.AssignCurrentEnvironment();
    }

    private SystemEnvironment AssignCurrentEnvironment()
    {
        return DistributedApplicationBuilder.Environment.EnvironmentName switch
        {
            "Development" => SystemEnvironment.Development,
            "Testing" => SystemEnvironment.Test,
            "TestingPersistent" => SystemEnvironment.TestPersistant,
            "Staging" => SystemEnvironment.Staging,
            "Production" => SystemEnvironment.Release,
            _ => SystemEnvironment.Development
        };
    }
}
