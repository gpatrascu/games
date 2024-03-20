using Games.Cards;

namespace games.grains.Events;

public record GameStartedEvent(Deck deck) : IGameEvent
{
    public Deck Deck { get; } = deck;
}