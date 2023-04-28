using Marten;
using Microsoft.AspNetCore.Mvc;
using Project;
using Project.ProviderShift;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProviderShiftAsyncController : ControllerBase
{
    private readonly IDocumentStore _documentStore;
    private readonly Guid KnownProvider1 = new("DC35FB28-3CD1-4C78-A3A5-A9CABA9C23B5");
    private readonly Guid KnownProvider2 = new("760DDBE1-EEF7-42E1-BCA8-FC2DAC827C03");

    public ProviderShiftAsyncController(IDocumentStore documentStore)
    {
        _documentStore = documentStore;
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var session = _documentStore.LightweightSession();
        var shift = session.Events.StartStream<ProviderShiftAsync>(
            new ProviderJoined(Guid.NewGuid(), KnownProvider2),
            new ProviderReady()
        );

        await session.SaveChangesAsync();
        return Ok(new { shift.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid shiftId)
    {
        var session = _documentStore.LightweightSession();
        var shift = await session.Events.AggregateStreamAsync<ProviderShiftAsync>(shiftId);
        if (shift == null) return NotFound();
        return Ok(shift);
    }
}