namespace Project.Aggregates;

public class ProviderShift
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public Guid BoardId { get; private set; }
    public Guid ProviderId { get; init; }
    public ProviderStatus Status { get; private set; }
    public string Name { get; init; }
    public Guid? AppointmentId { get; set; }

    // More here in just a minute...
}