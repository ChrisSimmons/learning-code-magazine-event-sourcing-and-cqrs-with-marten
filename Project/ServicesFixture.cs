using Marten;
using Marten.Events.Projections;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

namespace Project;

public class ServicesFixture
{
    private readonly ServiceProvider _serviceProvider;

    private const string ConnectionString =
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
        });

        _serviceProvider = services.BuildServiceProvider();
        MartenDocumentStore = _serviceProvider.GetRequiredService<IDocumentStore>();
    }

    public IDocumentStore MartenDocumentStore { get; }
}