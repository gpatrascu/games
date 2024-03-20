using Games.Cards;

namespace games.grains.Events;

public class DeckSuffledEvent
{
    public Deck Deck { get; }

    public DeckSuffledEvent(Deck deck)
    {
        Deck = deck;
    }
}