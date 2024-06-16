using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tutorial9.Context;
using tutorial9.DTO_s;
using tutorial9.Models;
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
        var client = await _context.Clients.FindAsync(id); //znajduje i zwraca klienta jak znajdzie, w przeciwnym wypadku =null

        if (client == null)
        {
            return "Client does not exist in db!";
        }

        var clientTrips = await _context.ClientTrips.Where(t =>
            t.IdClient == id).ToListAsync(); //ewentualnie można użyć .CountAsync()

        if (clientTrips.Capacity == 0)
        {
            return "Client has existing trips and cannot be removed!";
        }
        
        return "Client removed successfully!";
    }

    public async Task<string> AssignClientToTrip(int tripId, AddClientDto client)
    {
        var pesel = await _context.Clients.FirstOrDefaultAsync(c =>
            c.Pesel == client.Pesel);

        if (pesel != null)
        {
            return "Client already exists!";
        }

        var peselTrip = await _context.ClientTrips
            .FirstOrDefaultAsync(ct => ct.IdTrip == tripId && ct.IdClientNavigation.Pesel == client.Pesel);

        if (peselTrip != null)
        {
            return "Client is already assigned to a trip!";
        }

        var trip = await _context.Trips.FindAsync(tripId);

        if (trip == null)
        {
            return "Trip does not exist!";
        }

        if (DateTime.Now >= trip.DateFrom)
        {
            return "Trip has already started!";
        }

        var clientToAdd = new Client()
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
            Email = client.Email,
            Pesel = client.Pesel,
            Telephone = client.Telephone
        };
        _context.Clients.Add(clientToAdd);
        await _context.SaveChangesAsync();

        var existingClientTrip = await _context.ClientTrips.FindAsync(tripId);
        
        DateTime? paymentDate = existingClientTrip?.PaymentDate; //nullable

        var clientTripToAdd = new ClientTrip()
        {
            IdClient = clientToAdd.IdClient,
            IdTrip = tripId,
            RegisteredAt = DateTime.Now,
            PaymentDate = paymentDate
        };
        

        return "Client assigned to trip id " + tripId + "successfully!";
    }
}
