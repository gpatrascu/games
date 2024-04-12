using games.api.Azure;
using games.grains;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orleans.Serialization;
using Orleans.Storage;

namespace games.api.tests;

public class SiloConfigurator
{
    public void Configure(ISiloBuilder siloBuilder)
    {
        var options = new OrleansJsonSerializerOptions
        {
            JsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            }
        };
        var orleansJsonSerializer = new OrleansJsonSerializer(Options.Create(options));
        //siloBuilder.AddStateStorageBasedLogConsistencyProviderAsDefault();
        siloBuilder.AddLogStorageBasedLogConsistencyProviderAsDefault();

        var storageAccountConnectionString = Keys.GetStorageAccountConnectionString();

        siloBuilder.AddAdoNetGrainStorage("BiddingGameGrainStorageDB", options =>
        {
            options.GrainStorageSerializer = new JsonGrainStorageSerializer(orleansJsonSerializer);
            options.ConnectionString = @"Data Source=(LocalDb)\ITestSetupFixture;Initial Catalog=profilesDB;Integrated Security=SSPI;AttachDBFilename=C:\Users\g.patrascu\Profiles.mdf";
        });
        
        siloBuilder.AddAzureBlobGrainStorage(
            name: "BiddingGameGrainStorageBLob",
            configureOptions: options =>
            {
                options.ConfigureBlobServiceClient(
                    storageAccountConnectionString.Value.Value);
            });

        siloBuilder.AddAzureTableGrainStorage(
            name: "BiddingGameGrainStorageTable",
            configureOptions: options =>
            {
                options.GrainStorageSerializer = new JsonGrainStorageSerializer(orleansJsonSerializer); 
                options.ConfigureTableServiceClient(
                    storageAccountConnectionString.Value.Value);
            });
        siloBuilder.Services.AddSerializer(serializerBuilder =>
        {
            serializerBuilder.AddAssembly(typeof(GameState).Assembly);
            serializerBuilder.AddNewtonsoftJsonSerializer(_ => true);
        });
    }
}