
namespace Games.Api.IntegrationTests;


// generate API integration tests for the games.api project
public class CardsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    [Fact]
    public void ShouldCreateCard()
    {
        var response = await _httpClient.PostAsJsonAsync("/todos/v1", todo);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        Assert.NotNull(problemResult?.Errors);
        Assert.Collection(problemResult.Errors, (error) => Assert.Equal(errorMessage, error.Value.First()));
    }
}