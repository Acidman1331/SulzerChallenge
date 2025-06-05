using Moq;
using SulzerAirlines.Application.PriceCalculation;
using SulzerAirlines.Application.Services;
using SulzerAirlines.Application.Services.Interfaces;
using SulzerAirlines.Domain.Interfaces;
using SulzerAirlines.Domain.Models;
using Xunit;

namespace SulzerAirlines.Tests;

public class FlightServiceTests
{
    private readonly Mock<IFlightRepository> _repo;
    private readonly FlightService _service;
    private readonly Mock<IPriceCalculator> _priceCalculator;
    private readonly Mock<IFindRoutesService> _findrouteService;

    public FlightServiceTests()
    {
        _repo = new Mock<IFlightRepository>();
        _priceCalculator = new Mock<IPriceCalculator>();
        _findrouteService = new Mock<IFindRoutesService>();
        _service = new FlightService(_repo.Object, _priceCalculator.Object, _findrouteService.Object);
    }

    [Fact]
    public async Task GetCheapestRoute_ReturnsCheapest()
    {
        var allFlights = new List<FlightRoute>
        {
            new FlightRoute(new City("A"), new City("B"), 500, 0.1M, 20, 120),
            new FlightRoute(new City("A"), new City("B"), 500, 0.1M, 100, 220)
        };
        var routeOptions = new List<RouteOption>
        {
            new RouteOption(new List<FlightRoute>{ allFlights[0] }, 100),
            new RouteOption(new List<FlightRoute>{ allFlights[1] }, 200)
        };

        _repo.Setup(r => r.GetAllFlightsAsync()).ReturnsAsync(allFlights);
        _findrouteService.Setup(f => f.FindRoutesRecursive(
            It.IsAny<City>(), It.IsAny<City>(), It.IsAny<DateTime>(),
            allFlights, It.IsAny<List<FlightRoute>>(), It.IsAny<List<RouteOption>>(), It.IsAny<HashSet<string>>()))
            .Callback<City, City, DateTime, IReadOnlyList<FlightRoute>, List<FlightRoute>, List<RouteOption>, HashSet<string>>(
                (from, to, date, flights, path, results, visited) =>
                {
                    results.AddRange(routeOptions);
                })
            .Returns(Task.CompletedTask);

        var result = await _service.GetCheapestRouteAsync(new City("A"), new City("B"), DateTime.Now);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(100, result[0].TotalPrice);
    }

    [Fact]
    public async Task GetBestTimeToFlyAsync_ReturnsOrderedResults()
    {
        var routes = new List<FlightRoute>
        {
            new FlightRoute(new City("A"), new City("B"), 500, 0.1M, 20, 120)
        };
        _repo.Setup(r => r.GetRoutesAsync(It.IsAny<City>(), It.IsAny<City>())).ReturnsAsync(routes);
        _priceCalculator.Setup(p => p.CalculateFinalPriceAsync(It.IsAny<FlightRoute>(), It.IsAny<DateTime>()))
            .ReturnsAsync((FlightRoute r, DateTime d) => d.Hour * 10);

        var result = await _service.GetBestTimeToFlyAsync(new City("A"), new City("B"));

        Assert.NotNull(result);
        Assert.Equal(24, result.Count);
        Assert.True(result.SequenceEqual(result.OrderBy(r => r.Price)));
    }

    [Fact]
    public async Task GetRoundTripAsync_ReturnsCombinedRoutes()
    {
        var allFlights = new List<FlightRoute>
        {
            new FlightRoute(new City("A"), new City("B"), 500, 0.1M, 20, 120),
            new FlightRoute(new City("B"), new City("A"), 500, 0.1M, 20, 120)
        };
        var outbound = new List<RouteOption>
        {
            new RouteOption(new List<FlightRoute>{ allFlights[0] }, 100)
        };
        var inbound = new List<RouteOption>
        {
            new RouteOption(new List<FlightRoute>{ allFlights[1] }, 150)
        };

        _repo.Setup(r => r.GetAllFlightsAsync()).ReturnsAsync(allFlights);
        _findrouteService.Setup(f => f.FindRoutesWithMaxConnections(
            new City("A"), new City("B"), allFlights, 1, It.IsAny<List<FlightRoute>>(), It.IsAny<List<RouteOption>>(), It.IsAny<HashSet<string>>()))
            .Callback<City, City, IReadOnlyList<FlightRoute>, int, List<FlightRoute>, List<RouteOption>, HashSet<string>>(
                (from, to, flights, maxConn, path, results, visited) =>
                {
                    results.AddRange(outbound);
                })
            .Returns(Task.CompletedTask);

        _findrouteService.Setup(f => f.FindRoutesWithMaxConnections(
            new City("B"), new City("A"), allFlights, 1, It.IsAny<List<FlightRoute>>(), It.IsAny<List<RouteOption>>(), It.IsAny<HashSet<string>>()))
            .Callback<City, City, IReadOnlyList<FlightRoute>, int, List<FlightRoute>, List<RouteOption>, HashSet<string>>(
                (from, to, flights, maxConn, path, results, visited) =>
                {
                    results.AddRange(inbound);
                })
            .Returns(Task.CompletedTask);

        var result = await _service.GetRoundTripAsync(new City("A"), new City("B"), 1);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(250, result[0].TotalPrice);
        Assert.Equal(2, result[0].Flights.Count);
    }
}