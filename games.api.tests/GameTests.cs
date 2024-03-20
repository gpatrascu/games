using FluentAssertions;
using games.grains;
using Orleans.TestingHost;

namespace games.api.tests;

[Collection(ClusterCollection.Name)]
public class GameGrainTestsWithFixture(ClusterFixture fixture)
{
    private readonly TestCluster _cluster = fixture.Cluster;

    private static async Task AssertForPlayerByName(GameState state, string playerName)
    {
        if (state.Players.Select(player => player.Name).Any(result => result == playerName))
        {
            return;
        }

        Assert.True(false, $"Player {playerName} not found");
    }

    private async Task<IPlayerGrain> GetPlayerGrain(string name)
    {
        var player1 = _cluster.GrainFactory.GetGrain<IPlayerGrain>(Guid.NewGuid());
        await player1.SetName(name);
        return player1;
    }

    public class When_game_is_started : GameGrainTestsWithFixture
    {
        private GameState _state;
        private readonly IBiddingGameGrain? _game;

        public When_game_is_started(ClusterFixture fixture) : base(fixture)
        {
            _game = _cluster.GrainFactory.GetGrain<IBiddingGameGrain>(Guid.NewGuid());
            _game.Create().Wait();

            JoinPlayer(_game, "P1");
            JoinPlayer(_game, "P2");
            JoinPlayer(_game, "P3");

            _game.Start().Wait();
        }

        [Fact]
        public async Task Should_shuffle_and_deal_card()
        {
            var state = await _game.GetState();
            state.Players[0].Cards.Count.Should().Be(6);
            state.Players[1].Cards.Count.Should().Be(6);
            state.Players[2].Cards.Count.Should().Be(6);
            state.Deck.Cards.Count.Should().Be(3);
        }

        private void JoinPlayer(IBiddingGameGrain game, string name)
        {
            game.Join(GetPlayerGrain(name).Result).Wait();
        }
    }

    public class When_game_is_created(ClusterFixture fixture) : GameGrainTestsWithFixture(fixture)
    {
        [Fact]
        public async Task Create_gane()
        {
            var game = _cluster.GrainFactory.GetGrain<IBiddingGameGrain>(Guid.NewGuid());
            await game.Create();

            var state = await game.GetState();

            Assert.Equal(3, state.Players.Count);
            Assert.False(state.CanBeStarted);
        }

        [Fact]
        public async Task One_player_joined()
        {
            var game = _cluster.GrainFactory.GetGrain<IBiddingGameGrain>(Guid.NewGuid());
            await game.Create();

            var player = _cluster.GrainFactory.GetGrain<IPlayerGrain>(Guid.NewGuid());
            await player.SetName("P1");

            await game.Join(player);

            var state = await game.GetState();

            Assert.Contains(state.Players, player1 => player1.Name == "P1");
            Assert.False(state.CanBeStarted);
        }

        [Fact]
        public async Task Two_players_joined()
        {
            var game = _cluster.GrainFactory.GetGrain<IBiddingGameGrain>(Guid.NewGuid());
            await game.Create();

            await game.Join(await GetPlayerGrain("P1"));
            await game.Join(await GetPlayerGrain("P2"));

            var state = await game.GetState();

            await AssertForPlayerByName(state, "P1");
            await AssertForPlayerByName(state, "P2");
            Assert.False(state.CanBeStarted);
        }

        [Fact]
        public async Task Three_players_joined()
        {
            var game = _cluster.GrainFactory.GetGrain<IBiddingGameGrain>(Guid.NewGuid());
            await game.Create();

            await game.Join(await GetPlayerGrain("P1"));
            await game.Join(await GetPlayerGrain("P2"));
            await game.Join(await GetPlayerGrain("P3"));

            var state = await game.GetState();

            await AssertForPlayerByName(state, "P1");
            await AssertForPlayerByName(state, "P2");
            await AssertForPlayerByName(state, "P3");
            Assert.True(state.CanBeStarted);
        }
    }
}