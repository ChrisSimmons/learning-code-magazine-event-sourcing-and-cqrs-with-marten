namespace Project.ProviderShift;

public class ProviderShiftLive
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public Guid BoardId { get; protected set; }
    public Guid ProviderId { get; init; }
    public ProviderStatus Status { get; protected set; }
    public string Name { get; init; } = default!;
    public Guid? AppointmentId { get; set; }
    public static ProviderShiftLive Create(ProviderJoined joined)
    {
        return new ProviderShiftLive
        {
            Name = $"{"ProviderFN"} {"ProviderLN"}",
            Status = ProviderStatus.Ready,
            ProviderId = joined.ProviderId,
            BoardId = joined.BoardId
        };
    }

    public void Apply(ProviderReady ready)
    {
        AppointmentId = null;
        Status = ProviderStatus.Ready;
    }

    public void Apply(ProviderAssigned assigned)
    {
        AppointmentId = assigned.AppointmentId;
        Status = ProviderStatus.Assigned;
    }

    public void Apply(ChartingStarted charting)
    {
        Status = ProviderStatus.Charting;
    }
}