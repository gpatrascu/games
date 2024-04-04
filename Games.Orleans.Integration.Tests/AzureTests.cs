using games.api.Azure;
using Xunit.Abstractions;

namespace games.api.tests;

public class AzureTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AzureTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestKeyVault()
    {
        _testOutputHelper.WriteLine(Keys.GetStorageAccountConnectionString().Value.Value);

    }
}