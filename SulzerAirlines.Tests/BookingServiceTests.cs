using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SulzerAirlines.Domain.Models;
using SulzerAirlines.Application.Services;
using SulzerAirlines.Domain.Interfaces;
using SulzerAirlines.Application.PriceCalculation;

namespace SulzerAirlines.Tests;


public class BookingServiceTests
{
    private readonly Mock<IFlightRepository> _flightRepoMock = new();
    private readonly Mock<IPriceCalculator> _priceCalcMock = new();

    private BookingService CreateService() =>
        new BookingService(_flightRepoMock.Object, _priceCalcMock.Object);

    [Fact]
    public async Task BookFlightAsync_ReturnsFailure_WhenRouteNotFound()
    {
        _flightRepoMock.Setup(r => r.GetRouteAsync(It.IsAny<City>(), It.IsAny<City>(), It.IsAny<decimal>()))
            .ReturnsAsync((FlightRoute?)null);

        var service = CreateService();
        var result = await service.BookFlightAsync(new City("A"), new City("B"), DateTime.Now, 100, 1);

        Assert.False(result.Success);
        Assert.Contains("No se encontro ruta", result.Message);
        Assert.Equal(0, result.TotalPrice);
    }

    [Fact]
    public async Task BookFlightAsync_ReturnsFailure_WhenNotEnoughSeats()
    {
        var route = new FlightRoute(new City("A"), new City("B"), 100, 1, 0, 100);
        _flightRepoMock.Setup(r => r.GetRouteAsync(It.IsAny<City>(), It.IsAny<City>(), It.IsAny<decimal>()))
            .ReturnsAsync(route);

        var service = CreateService();
        var result = await service.BookFlightAsync(new City("A"), new City("B"), DateTime.Now, 100, 1);

        Assert.False(result.Success);
        Assert.Contains("No hay suficientes asientos", result.Message);
        Assert.Equal(0, result.TotalPrice);
    }

    [Fact]
    public async Task BookFlightAsync_ReturnsSuccess_WhenBookingIsValid()
    {
        var route = new FlightRoute(new City("A"), new City("B"), 100, 1, 10, 100);
        _flightRepoMock.Setup(r => r.GetRouteAsync(It.IsAny<City>(), It.IsAny<City>(), It.IsAny<decimal>()))
            .ReturnsAsync(route);
        _priceCalcMock.Setup(p => p.CalculateFinalPriceAsync(route, It.IsAny<DateTime>()))
            .ReturnsAsync(200);

        var service = CreateService();
        var result = await service.BookFlightAsync(new City("A"), new City("B"), DateTime.Now, 100, 2);

        Assert.True(result.Success);
        Assert.Contains("éxito", result.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(400, result.TotalPrice);
    }
}