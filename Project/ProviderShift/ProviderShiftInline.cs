namespace Project.ProviderShift;

// There's no difference here between 
public class ProviderShiftInline : ProviderShiftLive
{
    public static async Task<ProviderShiftInline> Create(ProviderJoined joined)
    {
        return new ProviderShiftInline
        {
            Name = $"{"ProviderFN"} {"ProviderLN"}",
            Status = ProviderStatus.Ready,
            ProviderId = joined.ProviderId,
            BoardId = joined.BoardId
        };
    }
}