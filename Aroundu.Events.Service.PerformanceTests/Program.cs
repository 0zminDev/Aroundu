using NBomber.CSharp;
using System.Net.Http.Json;

namespace Aroundu.Events.Service.PerformanceTests
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var targetUrl = Environment.GetEnvironmentVariable("EVENTS_API_URL");

            Console.WriteLine($"Bombarding Target: {targetUrl}");

            using var httpClient = new HttpClient();

            var scenario = Scenario.Create("simple_load_test", async context =>
                {
                    var body = JsonContent.Create(new { Name = "Load Test Event" });

                    var response = await httpClient.PostAsync($"{targetUrl}/api/events", body);

                    return response.IsSuccessStatusCode
                        ? Response.Ok(statusCode: ((int)response.StatusCode).ToString())
                        : Response.Fail(statusCode: ((int)response.StatusCode).ToString());
                })
                .WithoutWarmUp()
                .WithLoadSimulations(Simulation.KeepConstant(copies: 10, during: TimeSpan.FromSeconds(10)));

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();

            await Task.Delay(5000);
        }
    }
}
