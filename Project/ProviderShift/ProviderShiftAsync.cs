namespace Project.ProviderShift;

// There's no difference here between 
public class ProviderShiftAsync : ProviderShiftLive
{
    public static async Task<ProviderShiftAsync> Create(ProviderJoined joined)
    {
        return new ProviderShiftAsync
        {
            Name = $"{"ProviderFN"} {"ProviderLN"}",
            Status = ProviderStatus.Ready,
            ProviderId = joined.ProviderId,
            BoardId = joined.BoardId
        };
    }
}