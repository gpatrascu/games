using Newtonsoft.Json;

namespace Games.Cards;

public class Deck
{
    [JsonProperty]
    public IList<Card> Cards { get; private set; }

    public static Deck CreateStartingWith(CardValue minCardValue)
    {
        var cards = new List<Card>();
        for (int i = minCardValue; i <= CardValue.MaxValue; i++)
            foreach (var cardColor in CardColor.All)
            {
                var card = new Card(i, cardColor);
                cards.Add(card);
            }

        return new Deck
        {
            Cards = cards
        };
    }

    public Deck WithJokers(int numberOfJokers)
    {
        for (var i = 0; i < numberOfJokers; i++) Cards.Add(Card.Joker);

        return this;
    }

    public Deck Shuffle()
    {
        for (int i = 0; i < 10; i++)
        {
            OneSuffle();
        }

        return this;
    }

    private void OneSuffle()
    {
        var random = new Random();
        var n = Cards.Count;
        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (Cards[k], Cards[n]) = (Cards[n], Cards[k]);
        }
    }

    public Card Pop()
    {
        var card = Cards.First();
        Cards.RemoveAt(0);
        return card;
    }
}

public record Card
{
    public Card(CardValue value, CardColor color)
    {
        Value = value;
        Color = color;
    }

    private CardValue Value { get; }
    public CardColor Color { get; }
    public static Card Joker => new(CardValue.Joker, CardColor.Joker);

    public override string ToString()
    {
        return $"{Value},{Color}";
    }
}

public class CardValue
{
    private CardValue(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static CardValue Two => new(2);
    public static CardValue Three => new(3);
    public static CardValue Four => new(4);
    public static CardValue Five => new(5);
    public static CardValue Six => new(6);
    public static CardValue Seven => new(7);
    public static CardValue Eight => new(8);
    public static CardValue Nine => new(9);
    public static CardValue Ten => new(10);
    public static CardValue Jack => new(11);
    public static CardValue Queen => new(12);
    public static CardValue King => new(13);
    public static CardValue Ace => new(14);
    public static CardValue MaxValue => Ace;
    public static CardValue Joker => new(0);

    public static IList<CardValue> All => new List<CardValue>
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    };

    public static implicit operator int(CardValue cardValue)
    {
        return cardValue.Value;
    }

    public static implicit operator CardValue(int cardValue)
    {
        return All.First(v => v.Value == cardValue);
    }

    public override string ToString()
    {
        return $"{Value}";
    }
}

public class CardColor
{
    [JsonConstructor]
    private CardColor(string symbol)
    {
        Symbol = symbol;
    }

    private string Symbol { get; set; }

    public static CardColor Spade => new("♠");
    public static CardColor Heart => new("♥");
    public static CardColor Diamond => new("♦");
    public static CardColor Club => new("♣");
    public static CardColor Joker => new("Joker");

    public static IEnumerable<CardColor> All => new List<CardColor> { Spade, Heart, Diamond, Club };

    public override string ToString()
    {
        return $"{Symbol}";
    }
}