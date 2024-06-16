using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tutorial9.Context;
using tutorial9.DTO_s;
using tutorial9.Services;

public class TripsService : ITripsService
{
    private readonly TripsContext _context;

    public TripsService(TripsContext context) 
    {
        _context = context;
    }

    public async Task<TripInfoDto> GetTrips(string? query, int? page, int? pageSize)
    {
        // Ustal domyślne wartości, jeśli parametry nie zostały podane
        int currentPage = page ?? 1;
        int currentPageSize = pageSize ?? 10;

        // Pobierz dane, filtrowanie i sortowanie
        var tripsQuery = _context.Trips
            .Include(t => t.IdCountries) // Załadowanie krajów
            .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation) // Załadowanie klientów
            .AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            tripsQuery = tripsQuery.Where(t => t.Name.Contains(query) || t.Description.Contains(query));
        }

        tripsQuery = tripsQuery.OrderByDescending(t => t.DateFrom);

        // Oblicz ile elementów należy pominąć
        int skip = (currentPage - 1) * currentPageSize;

        // Pobierz dane z uwzględnieniem stronicowania
        var trips = await tripsQuery.Skip(skip).Take(currentPageSize).ToListAsync();

        // Przekształć dane do DTO
        var tripsDto = trips.Select(t => new TripDto
        {
            Name = t.Name,
            Description = t.Description,
            DateFrom = t.DateFrom,
            DateTo = t.DateTo,
            MaxPeople = t.MaxPeople,
            Countries = t.IdCountries.Select(c => new CountryDto
            {
                Name = c.Name
            }).ToList(),
            Clients = t.ClientTrips.Select(ct => new ClientDto
            {
                FirstName = ct.IdClientNavigation.FirstName,
                LastName = ct.IdClientNavigation.LastName,
            }).ToList()
        }).ToList();

        // Oblicz całkowitą liczbę stron
        int totalItems = await tripsQuery.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalItems / currentPageSize);

        return new TripInfoDto
        {
            PageNum = currentPage,
            PageSize = currentPageSize,
            AllPages = totalPages,
            Trips = tripsDto
        };
    }

    public async Task<string> DeleteClient(int id)
    {
        return "Client removed successfully!";
    }

    public async Task<string> AssignClientToTrip(int tripId)
    {
        return "Client assigned to trip id " + tripId + "successfully!";
    }
}
