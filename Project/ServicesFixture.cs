using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Project;

public class ServicesFixture : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;

    public ServicesFixture()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddExampleMarten();
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