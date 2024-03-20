using Games.Cards;
using games.grains.Events;

namespace games.grains;


public class GameState
{
    public GameState()
    {
        Players = new List<Player>();
    }

    public IList<Player> Players { get; }

    public bool CanBeStarted { get; set; }
    
    public Deck Deck { get; set; }

    public void Apply(GameCreatedEvent @event)
    {
        Players.Add(new Player());
        Players.Add(new Player());
        Players.Add(new Player());
    }
    
    public void Apply(PlayerJoinedEvent @event)
    {
        var first = Players.First(player => player.IsEmpty);
        
        first.AddPlayer(@event.Player);

        if (Players.Count(player => !player.IsEmpty) == 3)
        {
            CanBeStarted = true;
        }
    }
    
    public void Apply(GameStartedEvent @event)
    {
        Deck = @event.Deck;
        DealCards();
    }

    private void DealCards()
    {
        for (var i = 0; i < 6; i++)
        {
            Players[0].Cards.Add(Deck.Pop());
            Players[1].Cards.Add(Deck.Pop());
            Players[2].Cards.Add(Deck.Pop());
        }
    }
}