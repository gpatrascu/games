using games.grains;
using Microsoft.Extensions.Configuration;
using Orleans.Serialization;
using Orleans.TestingHost;

namespace games.api.tests;

public class TestClientBuilderConfigurator : IClientBuilderConfigurator
{
    public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
    {
        clientBuilder.Services.AddSerializer(serializerBuilder =>
        {
            serializerBuilder.AddAssembly(typeof(GameState).Assembly);
            serializerBuilder.AddNewtonsoftJsonSerializer(_ => true);
        });
    }
}

public class TestSiloConfigurator : ISiloConfigurator
{
    public void Configure(ISiloBuilder siloBuilder)
    {
        new SiloConfigurator().Configure(siloBuilder);
    }
}