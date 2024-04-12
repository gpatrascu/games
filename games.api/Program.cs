using games.api.tests;
using games.grains;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(static siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    // siloBuilder.AddMemoryGrainStorage("urls");
    
    new SiloConfigurator().Configure(siloBuilder);
});

using var app = builder.Build();

app.MapPost("/games", static async (IGrainFactory grains, StartGame startGame) =>
{
    var game = grains.GetGrain<IBiddingGameGrain>(Guid.NewGuid()); 
    await game.Create();
    return Results.Created();
});


app.Run();



public partial class Program
{ }

public class StartGame
{
    
};