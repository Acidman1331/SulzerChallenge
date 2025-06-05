using SulzerAirlines.Domain.Models;

namespace SulzerAirlines.Application.Services.Interfaces
{
    public interface IFindRoutesService
    {
        Task FindRoutesWithMaxConnections(City current, City destination, IReadOnlyList<FlightRoute> allFlights,
        int maxConnections, List<FlightRoute> currentPath, List<RouteOption> results, HashSet<string> visited);
        Task FindRoutesRecursive(City current, City destination, DateTime date,
            IReadOnlyList<FlightRoute> allFlights, List<FlightRoute> currentPath, List<RouteOption> results, HashSet<string> visited);
    }
}