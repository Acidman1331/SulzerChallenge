using SulzerAirlines.Application.PriceCalculation;
using SulzerAirlines.Application.Services.Interfaces;
using SulzerAirlines.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SulzerAirlines.Application.Services
{

    public class FindRoutesService : IFindRoutesService
    {
        private readonly IPriceCalculator _priceCalculator;

        public FindRoutesService(IPriceCalculator priceCalculator)
        {

            _priceCalculator = priceCalculator;
        }
        public async Task FindRoutesWithMaxConnections(City current, City destination, IReadOnlyList<FlightRoute> allFlights,
        int maxConnections, List<FlightRoute> currentPath, List<RouteOption> results, HashSet<string> visited)
        {
            maxConnections = maxConnections == 0 ? maxConnections + 1 : maxConnections;
            if (current.Name == destination.Name && currentPath.Count <= maxConnections && currentPath.Count > 0)
            {
                int totalPrice = 0;
                foreach (var flight in currentPath)
                    totalPrice += await _priceCalculator.CalculateFinalPriceAsync(flight, DateTime.UtcNow);

                results.Add(new RouteOption(new List<FlightRoute>(currentPath), totalPrice));
                return;
            }

            if (currentPath.Count > maxConnections) return;

            visited.Add(current.Name);

            var nextFlights = allFlights.Where(f => f.From.Name == current.Name && !visited.Contains(f.To.Name)).ToList();

            foreach (var nextFlight in nextFlights)
            {
                currentPath.Add(nextFlight);
                await FindRoutesWithMaxConnections(nextFlight.To, destination, allFlights, maxConnections, currentPath, results, visited);
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            visited.Remove(current.Name);
        }

        public async Task FindRoutesRecursive(City current, City destination, DateTime date,
            IReadOnlyList<FlightRoute> allFlights, List<FlightRoute> currentPath, List<RouteOption> results, HashSet<string> visited)
        {
            if (current.Name == destination.Name && currentPath.Count > 0)
            {
                int totalPrice = 0;
                foreach (var flight in currentPath)
                    totalPrice += await _priceCalculator.CalculateFinalPriceAsync(flight, date);
                results.Add(new RouteOption(new List<FlightRoute>(currentPath), totalPrice));
                return;
            }

            visited.Add(current.Name);

            var nextFlights = allFlights.Where(f => f.From.Name == current.Name && !visited.Contains(f.To.Name)).ToList();

            foreach (var nextFlight in nextFlights)
            {
                currentPath.Add(nextFlight);
                await FindRoutesRecursive(nextFlight.To, destination, date, allFlights, currentPath, results, visited);
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            visited.Remove(current.Name);
        }
    }
}
