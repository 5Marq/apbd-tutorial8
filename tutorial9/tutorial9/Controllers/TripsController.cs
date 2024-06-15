using Microsoft.AspNetCore.Mvc;
using tutorial9.Services;

namespace tutorial9.Controllers;

[Route("api/trips")]
[ApiController]
public class TripsController : ControllerBase
{
    private ITripsService _tripsService;

    public TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips(string? query, int? page, int? pageSize) // '?' - nullable
    {
        var result = await _tripsService.GetTrips(query, page, pageSize);
        return StatusCode(StatusCodes.Status201Created);
    }
}