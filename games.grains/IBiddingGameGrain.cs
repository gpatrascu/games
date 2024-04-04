using Games.Cards;
using games.grains.Events;
using Orleans.EventSourcing;
using Orleans.Providers;

namespace games.grains;

public interface IBiddingGameGrain : IGrainWithGuidKey
{
    Task Create();
    Task<GameState> GetState();
    Task Join(IPlayerGrain player);
    Task Start();
    Task<IReadOnlyList<object>> GetEvents();
}

[StorageProvider(ProviderName = "BiddingGameGrainStorageDB")]
public class BiddingGameGrain : JournaledGrain<GameState>, IBiddingGameGrain
    //,ICustomStorageInterface<GameState, object>
{
    public async Task Create()
    {
        RaiseEvent(new GameCreatedEvent());
        await ConfirmEvents();
    }

    public Task<GameState> GetState()
    {
        return Task.FromResult(State);
    }

    public async Task Join(IPlayerGrain player)
    {
        await player.JoinGame(GrainReference);

        var name = await player.GetName();
        var playerReference = new PlayerReference(player.GetPrimaryKey(), name);
        RaiseEvent(new PlayerJoinedEvent(playerReference));
        await ConfirmEvents();
    }

    public async Task Start()
    {   
        var deck = Deck.CreateStartingWith(CardValue.Ten).WithJokers(1).Shuffle();
        
        RaiseEvent(new GameStartedEvent(deck));
        await ConfirmEvents();
    }

    public async Task<IReadOnlyList<object>> GetEvents()
    {
        var version = Version; 
        return await RetrieveConfirmedEvents(0, version);
    }
}

public record PlayerReference(Guid Id, string Name)
{
}