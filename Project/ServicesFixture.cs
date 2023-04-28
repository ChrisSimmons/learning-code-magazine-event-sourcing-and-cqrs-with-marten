using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Project;

public class ServicesFixture
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=event_sourcing_cqrs_marten;Username=postgres;Password=postgres;";
    
    public ServicesFixture()
    {
        var services = new ServiceCollection();
        
        services.AddMarten(opts =>
        {
            opts.Connection(ConnectionString);
        });
    }
}