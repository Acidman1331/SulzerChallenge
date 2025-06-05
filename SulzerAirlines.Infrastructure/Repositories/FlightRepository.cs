namespace SulzerAirlines.Infrastructure.Repositories;

using SulzerAirlines.Domain.Models;
using SulzerAirlines.Domain.Interfaces;
using System.Collections.Generic;
using System;

/// <summary>
/// Repositorio "in-memory" simulado.
/// </summary>
public class FlightRepository : IFlightRepository
{
    private static readonly List<FlightRoute> _routes = new()
    {
        new FlightRoute(new City("A"), new City("B"), 500, 0.1M, 20, 120),
        new FlightRoute(new City("A"), new City("B"), 500, 0.1M, 100, 220),
        new FlightRoute(new City("A"), new City("C"), 800, 0.3M, 120, 143),
        new FlightRoute(new City("B"), new City("A"), 500, 0.1M, 120, 110),
        new FlightRoute(new City("B"), new City("C"), 300, 0.2M, 120, 110),
        new FlightRoute(new City("C"), new City("B"), 300, 0.4M, 120, 110),
        new FlightRoute(new City("C"), new City("A"), 800, 0.6M, 20, 310),
        new FlightRoute(new City("C"), new City("A"), 800, 0.2M, 100, 190),
    };

    public Task<IReadOnlyList<FlightRoute>> GetRoutesAsync(City from, City to)
    {
        return Task.FromResult<IReadOnlyList<FlightRoute>>(_routes.Where(r => r.From.Equals(from) && r.To.Equals(to)).ToList());
    }

    public Task<IReadOnlyList<FlightRoute>> GetAllFlightsAsync()
    {
        return Task.FromResult<IReadOnlyList<FlightRoute>>(_routes.ToList());
    }

    public Task<FlightRoute> GetRouteAsync(City from, City to, decimal basePrice)
    {
        var route = _routes.Where(r => r.From.Equals(from) && r.To.Equals(to) && r.BasePrice.Equals(basePrice)).FirstOrDefault();
        return Task.FromResult<FlightRoute>(route);
    }
}
