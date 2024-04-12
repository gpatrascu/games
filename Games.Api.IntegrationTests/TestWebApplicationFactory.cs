using games.api.tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Games.Api.IntegrationTests;

public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
           
        });
        builder.UseOrleans(siloBuilder =>
            {
                siloBuilder.UseLocalhostClustering();
                new SiloConfigurator().Configure(siloBuilder);
            });

        return base.CreateHost(builder);
    }
}