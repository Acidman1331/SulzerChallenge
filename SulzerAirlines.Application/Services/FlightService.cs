using SulzerAirlines.Application.PriceCalculation;
using SulzerAirlines.Application.Services.Interfaces;
using SulzerAirlines.Domain.Interfaces;
using SulzerAirlines.Domain.Models;
using SulzerAirlines.Infrastructure.Repositories;
using System.Collections.Generic;
namespace SulzerAirlines.Application.Services;

public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;
    private readonly IPriceCalculator _priceCalculator;
    private readonly IFindRoutesService _findRoutesService;

    public FlightService(IFlightRepository repository, IPriceCalculator priceCalculator, IFindRoutesService findRoutesService)
    {
        _flightRepository = repository;
        _priceCalculator = priceCalculator;
        _findRoutesService = findRoutesService;
    }

    // 1. Consulta ruta más económica entre dos ciudades (puede tener conexiones)
    public async Task<IReadOnlyList<RouteOption>> GetCheapestRouteAsync(City from, City to, DateTime date)
    {
        var allFlights = await _flightRepository.GetAllFlightsAsync();

        var routes = new List<RouteOption>();
        await _findRoutesService.FindRoutesRecursive(from, to, date, allFlights, new List<FlightRoute>(), routes, new HashSet<string>());

        if (routes.Count == 0) return Array.Empty<RouteOption>();

        var minPrice = routes.Min(r => r.TotalPrice);
        return routes.Where(r => r.TotalPrice == minPrice).ToList();
    }

    //2. Consulta mejores franjas horarias para volar entre dos ciudades ordenadas de menor a mayor
    public async Task<List<BestTimeToFlyResult>> GetBestTimeToFlyAsync(City from, City to)
    {
        var routes = await _flightRepository.GetRoutesAsync(from, to);

        var hours = Enumerable.Range(0, 24).Select(h => new TimeOnly(h, 0));

        var prices = new List<BestTimeToFlyResult>();
        foreach (var time in hours)
        {
            var priceTasks = routes
                .Select(r => _priceCalculator.CalculateFinalPriceAsync(r, DateTime.Today.Add(time.ToTimeSpan())))
                .ToList();

            if (priceTasks.Count == 0) continue;

            var results = await Task.WhenAll(priceTasks);
            int minPrice = results.Min();

            prices.Add(new BestTimeToFlyResult { Time = time, Price = minPrice });
        }

        return prices.OrderBy(p => p.Price).ToList();
    }

    // 3. Consulta vuelos de ida y vuelta con al menos una conexión (o más si se especifica)
    public async Task<IReadOnlyList<RouteOption>> GetRoundTripAsync(City from, City to, int maxConnections)
    {
        var allFlights = await _flightRepository.GetAllFlightsAsync();

        var outboundRoutes = new List<RouteOption>();
        await _findRoutesService.FindRoutesWithMaxConnections(from, to, allFlights, maxConnections, new List<FlightRoute>(), outboundRoutes, new HashSet<string>());

        var returnRoutes = new List<RouteOption>();
        await _findRoutesService.FindRoutesWithMaxConnections(to, from, allFlights, maxConnections, new List<FlightRoute>(), returnRoutes, new HashSet<string>());

        var combinedRoutes = new List<RouteOption>();

        foreach (var outRoute in outboundRoutes)
        {
            foreach (var retRoute in returnRoutes)
            {
                var combinedFlights = new List<FlightRoute>();
                combinedFlights.AddRange(outRoute.Flights);
                combinedFlights.AddRange(retRoute.Flights);
                int totalPrice = outRoute.TotalPrice + retRoute.TotalPrice;
                combinedRoutes.Add(new RouteOption(combinedFlights, totalPrice));
            }
        }

        return combinedRoutes;
    }

    public async Task<IReadOnlyList<FlightRoute>> GetRoutesAsync(City from, City to)
    {
        return await _flightRepository.GetRoutesAsync(from, to);
    }
}

