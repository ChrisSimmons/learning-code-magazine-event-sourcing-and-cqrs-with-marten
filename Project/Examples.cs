using FluentAssertions;
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

    [Fact]
    public async Task access_live_aggregation()
    {
        // We need to dictate the shift ID if we are to look it up later in the test
        var shiftId = Guid.NewGuid();

        _session.Events.StartStream<ProviderShift>(shiftId,
            new ProviderJoined(Guid.NewGuid(), KnownProvider1),
            new ProviderReady()
        );

        await _session.SaveChangesAsync();

        var querySession = _fixture.MartenDocumentStore.QuerySession();

        // Fetch all the events for the stream, and
        // apply them to a ProviderShift aggregate
        var shift = await querySession
            .Events
            .AggregateStreamAsync<ProviderShift>(shiftId);

        shift.Version.Should().Be(2);
        shift.ProviderId.Should().Be(KnownProvider1);
    }
}