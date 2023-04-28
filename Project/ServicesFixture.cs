using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

namespace Project;

public class ServicesFixture : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;

    public const string ConnectionString =
        "Host=localhost;Port=5432;Database=event_sourcing_cqrs_marten;Username=postgres;Password=postgres;";

    public ServicesFixture()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddMarten(opts =>
        {
            opts.Connection(ConnectionString);
            opts.AutoCreateSchemaObjects = AutoCreate.All;

            opts.Projections.SelfAggregate<ProviderShift.ProviderShiftLive>
                (ProjectionLifecycle.Live);
            opts.Projections.SelfAggregate<ProviderShift.ProviderShiftInline>
                (ProjectionLifecycle.Inline);
            opts.Projections.SelfAggregate<ProviderShift.ProviderShiftAsync>
                (ProjectionLifecycle.Async);
        }).AddAsyncDaemon(DaemonMode.HotCold);

        _serviceProvider = services.BuildServiceProvider();
        MartenDocumentStore = _serviceProvider.GetRequiredService<IDocumentStore>();
    }

    public IDocumentStore MartenDocumentStore { get; }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
    }
}