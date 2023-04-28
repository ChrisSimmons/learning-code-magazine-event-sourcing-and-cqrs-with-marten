using Marten;
using Project.Aggregates;

namespace Project;

public class Examples : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _fixture;
    private readonly IDocumentSession _session;

    private readonly Guid KnownProvider1 = new("DC35FB28-3CD1-4C78-A3A5-A9CABA9C23B5");
    private readonly Guid KnownProvider2 = new("760DDBE1-EEF7-42E1-BCA8-FC2DAC827C03");

    public Examples(ServicesFixture fixture)
    {
        _fixture = fixture;

        _session = _fixture.MartenDocumentStore.OpenSession();
    }

    [Fact]
    public void Clean()
    {
        // Uncomment to allow clean
        //_fixture.MartenDocumentStore.Advanced.Clean.CompletelyRemoveAll();
    }

    [Fact]
    public async Task FirstExample()
    {
        // This would be an input
        var boardId = Guid.NewGuid();

        _session.Events.StartStream<ProviderShift>(
            new ProviderJoined(boardId, KnownProvider1),
            new ProviderReady()
        );

        await _session.SaveChangesAsync();
    }

}