namespace SulzerAirlines.Domain.Interfaces;

using SulzerAirlines.Domain.Models;

public interface IFlightRepository
{
    Task<IReadOnlyList<FlightRoute>> GetRoutesAsync(City from, City to);
    Task<IReadOnlyList<FlightRoute>> GetAllFlightsAsync();
    Task<FlightRoute> GetRouteAsync(City from, City to, decimal basePrice);
}



