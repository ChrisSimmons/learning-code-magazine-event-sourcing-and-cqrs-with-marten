using Marten;
using Microsoft.AspNetCore.Mvc;
using Project;
using Project.Projections;
using Project.ProviderShift;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProviderShiftAsyncController : ControllerBase
{
    private readonly IDocumentStore _documentStore;
    private readonly Guid _knownProvider1 = new("DC35FB28-3CD1-4C78-A3A5-A9CABA9C23B5");
    private readonly Guid _knownProvider2 = new("760DDBE1-EEF7-42E1-BCA8-FC2DAC827C03");

    public ProviderShiftAsyncController(IDocumentStore documentStore)
    {
        _documentStore = documentStore;
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var session = _documentStore.LightweightSession();
        var shift = session.Events.StartStream<ProviderShiftAsync>(
            new ProviderJoined(Guid.NewGuid(), _knownProvider2),
            new ProviderReady()
        );

        await session.SaveChangesAsync();
        return Ok(new { shift.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid shiftId, long? version)
    {
        var session = _documentStore.LightweightSession();
        ProviderShiftAsync? shift;
        if (version.HasValue)
        {
            shift = await session.Events.AggregateStreamAsync<ProviderShiftAsync>(shiftId, version: version.Value);
        }
        else
        {
            shift = await session.Events.AggregateStreamAsync<ProviderShiftAsync>(shiftId);
        }

        if (shift == null) return NotFound();
        return Ok(shift);
    }

    [HttpPost("reset-data")]
    public async Task<IActionResult> ResetProjections()
    {
        var daemon = await _documentStore.BuildProjectionDaemonAsync();
        await daemon.RebuildProjectionAsync<AnotherProjection>(CancellationToken.None);
        return NoContent();
    }
}