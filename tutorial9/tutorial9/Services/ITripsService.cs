using tutorial9.DTO_s;

namespace tutorial9.Services;

public interface ITripsService
{
    public Task<TripInfoDto> GetTrips(string? query, int? page, int? pageSize);
    public Task<string> DeleteClient(int id);
    public Task<string> AssignClientToTrip(int clientId, AddClientDto client);
}