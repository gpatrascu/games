using FluentAssertions;
using Xunit.Abstractions;

namespace Games.Cards.Tests;

public class DeckTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public DeckTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void NumberOfCardsFrom2()
    {
        var deck = Deck.CreateStartingWith(CardValue.Two);
        deck.Cards.Count().Should().Be(52);
    }
    
    [Fact]
    public void NumberOfCardsFrom10()
    {
        var deck = Deck.CreateStartingWith(CardValue.Ten);
        deck.Cards.Count().Should().Be(20);
    }
    
    [Fact]
    public void NumberOfCardsFrom10AndAJoker()
    {
        var deck = Deck.CreateStartingWith(CardValue.Ten)
            .WithJokers(1);
        deck.Cards.Count().Should().Be(21);
    }

    [Fact]
    public void Suffle()
    {
        var deck = Deck.CreateStartingWith(CardValue.Ten)
            .WithJokers(1);

        List<Card> beforeSuffle = deck.Cards.ToList();

        deck.Shuffle();
        
        deck.Cards.Should().BeEquivalentTo(beforeSuffle);
        int numberOfDifferences = 0;
        for (int i = 0; i < beforeSuffle.Count; i++)
        {
            if (beforeSuffle[i] != deck.Cards[i])
            {
                numberOfDifferences++;
            }
        }

        _testOutputHelper.WriteLine(numberOfDifferences.ToString());
        numberOfDifferences.Should().BeGreaterThan(deck.Cards.Count / 2);
    }
}