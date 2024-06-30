using FluentAssertions;
using Marten;

namespace Project;

public class Examples : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _fixture;
    private readonly IDocumentSession _session;

    private readonly Guid _knownProvider1 = new("DC35FB28-3CD1-4C78-A3A5-A9CABA9C23B5");
    private readonly Guid _knownProvider2 = new("760DDBE1-EEF7-42E1-BCA8-FC2DAC827C03");

    public Examples(ServicesFixture fixture)
    {
        _fixture = fixture;

        _session = _fixture.MartenDocumentStore.LightweightSession();
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

        _session.Events.StartStream<ProviderShift.ProviderShiftLive>(
            new ProviderJoined(boardId, _knownProvider1),
            new ProviderReady()
        );
        
        await _session.SaveChangesAsync();
    }

    [Fact]
    public async Task AccessLiveAggregation()
    {
        // We need to dictate the shift ID if we are to look it up later in the test
        var createdShiftStream = _session.Events.StartStream<ProviderShift.ProviderShiftLive>(
            new ProviderJoined(Guid.NewGuid(), _knownProvider1),
            new ProviderReady()
        );

        await _session.SaveChangesAsync();

        var querySession = _fixture.MartenDocumentStore.QuerySession();

        // Fetch all the events for the stream, and
        // apply them to a ProviderShift aggregate
        var queriedShift = await querySession
            .Events
            .AggregateStreamAsync<ProviderShift.ProviderShiftLive>(createdShiftStream.Id);

        queriedShift!.Version.Should().Be(2);
        queriedShift.ProviderId.Should().Be(_knownProvider1);
    }

    [Fact]
    public async Task AccessInlineAggregation()
    {
        var createdShiftStream =
            _session.Events.StartStream<ProviderShift.ProviderShiftInline>(
                new ProviderJoined(Guid.NewGuid(), _knownProvider2),
                new ProviderReady()
            );

        await _session.SaveChangesAsync();

        var querySession = _fixture.MartenDocumentStore.QuerySession();

        // Load the persisted ProviderShift right out of the database
        var shift = await querySession.LoadAsync<ProviderShift.ProviderShiftInline>(createdShiftStream.Id);

        shift!.Version.Should().Be(2);
        shift.ProviderId.Should().Be(_knownProvider2);
    }
}