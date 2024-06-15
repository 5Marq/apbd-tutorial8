using tutorial9.Context;
using tutorial9.DTO_s;

namespace tutorial9.Services;

public class TripsService : ITripsService
{
    private TripsContext _context;

    public TripsService(TripsContext context)
    {
        _context = context;
    }
    public async Task<TripInfoDto> GetTrips(string? query, int? page, int? pageSize)
    {
        return null;
    }
}