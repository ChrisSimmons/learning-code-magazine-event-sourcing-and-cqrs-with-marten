namespace Project.Projections;

public class AnotherProjection
{
    public Guid Id { get; set; }
    public string SomeProperty { get; private set; }

    public void Apply(ProviderReady @event)
    {
        SomeProperty = $"{SomeProperty}{nameof(ProviderReady)}";
    }

    public void Apply(ProviderAssigned @event)
    {
        SomeProperty = $"{SomeProperty}{nameof(ProviderAssigned)}";
    }

    public void Apply(ChartingStarted @event)
    {
        SomeProperty = $"{SomeProperty}{nameof(ChartingStarted)}";
    }

    public void Apply(ProviderJoined @event)
    {
        SomeProperty = $"{SomeProperty}{nameof(ProviderJoined)}";
    }
}