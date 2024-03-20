using Orleans.TestingHost;

namespace games.api.tests;

public sealed class ClusterFixture : IDisposable
{
    public TestCluster Cluster { get; } = new TestClusterBuilder()
        .AddClientBuilderConfigurator<TestClientBuilderConfigurator>()
        .AddSiloBuilderConfigurator<TestSiloConfigurator>()
        .Build();

    public ClusterFixture() => Cluster.Deploy();

    void IDisposable.Dispose() => Cluster.StopAllSilos();
}