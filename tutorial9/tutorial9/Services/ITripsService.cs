using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tutorial9.DTO_s;

namespace tutorial9.Services;

public interface ITripsService
{
    public Task<TripInfoDto> GetTrips(string? query, int? page, int? pageSize);
}