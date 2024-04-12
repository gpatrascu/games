using System.Net.Http.Json;
using Games.Api.IntegrationTests;

public class CardsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CardsTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateGame()
    {
        // Act
        var response = await _client.PostAsJsonAsync("games", new StartGame());

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
}