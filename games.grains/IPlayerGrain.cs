using Orleans;
using Orleans.Runtime;

namespace games.grains;

public interface IPlayerGrain : IGrainWithGuidKey
{
    Task<string> GetName();
    Task SetName(string name);
    Task JoinGame(GrainReference biddingGameGrain);
}

public class PlayerGrain : IPlayerGrain
{
    private string _name;

    public Task<string> GetName()
    {
        return Task.FromResult(_name);
    }

    public Task SetName(string name)
    {
        _name = name;
        return Task.CompletedTask;
    }

    public Task JoinGame(GrainReference biddingGameGrain)
    {
        return Task.CompletedTask;
    }
}