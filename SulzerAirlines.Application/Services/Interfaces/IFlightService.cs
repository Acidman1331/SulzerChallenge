using SulzerAirlines.Domain.Models;

namespace SulzerAirlines.Application.Services.Interfaces;

public interface IFlightService
{
    Task<IReadOnlyList<RouteOption>> GetCheapestRouteAsync(City from, City to, DateTime date);
    Task<IReadOnlyList<RouteOption>> GetRoundTripAsync(City from, City to, int maxConnections);
    Task<List<BestTimeToFlyResult>> GetBestTimeToFlyAsync(City from, City to);
}

public interface ISeatManagerService
{
    Task<bool> HasSeatsAsync(string from, string to, int requestedSeats);
    Task ReserveSeatsAsync(string from, string to, int seats);
}


