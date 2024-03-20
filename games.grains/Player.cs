using Games.Cards;
using Newtonsoft.Json;

namespace games.grains;

public class Player
{
    [JsonProperty]
    public Guid PlayerId { get; private set; }
    
    [JsonProperty]
    public string Name { get; private set; }
    
    public bool IsEmpty => PlayerId == Guid.Empty;
    public IList<Card> Cards { get; } = new List<Card>();

    public void AddPlayer(PlayerReference eventPlayer)
    {
        PlayerId = eventPlayer.Id;
        Name = eventPlayer.Name;
    }
}