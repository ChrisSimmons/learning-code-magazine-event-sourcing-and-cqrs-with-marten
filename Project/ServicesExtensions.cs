using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

namespace Project;

public static class ServicesExtensions
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=event_sourcing_cqrs_marten;Username=postgres;Password=postgres;";

    public static MartenServiceCollectionExtensions.MartenConfigurationExpression AddExampleMarten(
        this IServiceCollection services)
    {
        var result = services.AddMarten(opts =>
        {
            opts.Connection(ConnectionString);
            opts.AutoCreateSchemaObjects = AutoCreate.All;

            opts.Projections.SelfAggregate<ProviderShift.ProviderShiftLive>
                (ProjectionLifecycle.Live);
            opts.Projections.SelfAggregate<ProviderShift.ProviderShiftInline>
                (ProjectionLifecycle.Inline);
            opts.Projections.SelfAggregate<ProviderShift.ProviderShiftAsync>
                (ProjectionLifecycle.Async);
            opts.Projections.SelfAggregate<Projections.AnotherProjection>
                (ProjectionLifecycle.Async);
            
        });
        
        result.AddAsyncDaemon(DaemonMode.HotCold);

        return result;
    }
    
}