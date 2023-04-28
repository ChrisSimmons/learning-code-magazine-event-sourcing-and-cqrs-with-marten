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

    public static async Task<ProviderShift> Create(ProviderJoined joined)
    {
        //var p = await session.LoadAsync<Provider>(joined.ProviderId);

        return new ProviderShift
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

    // This is kind of a catch all for any paperwork
    // the provider has to do after an appointment
    // for the just concluded appointment
    public void Apply(ChartingStarted charting)
    {
        Status = ProviderStatus.Charting;
    }
}