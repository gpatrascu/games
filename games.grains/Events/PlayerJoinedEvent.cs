namespace games.grains;

public class PlayerJoinedEvent(PlayerReference player)
{
    public PlayerReference Player { get; set; } = player;
}