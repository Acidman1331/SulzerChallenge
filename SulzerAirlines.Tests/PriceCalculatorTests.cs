using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SulzerAirlines.Application.PriceCalculation;
using SulzerAirlines.Domain.Models;

namespace SulzerAirlines.Tests;

public class PriceCalculatorTests
{
    [Fact]
    public async Task CalculateFinalPriceAsync_ReturnsExpectedPrice()
    {
        // Arrange
        var route = new FlightRoute(new City("A"), new City("B"), 1000, 0.2M, 10, 150);
        var date = new DateTime(2025, 6, 5, 10, 0, 0);

        var factorProviderMock = new Mock<ITimeFactorProviderFactory>();
        factorProviderMock.Setup(f => f.GetFactor(date)).Returns(1.5M);

        // Suponiendo que tu PriceCalculator usa el factor así:
        // precioFinal = BasePrice * BaseFactor * factorHora
        var calculator = new PriceCalculator(factorProviderMock.Object);

        // Act
        var result = await calculator.CalculateFinalPriceAsync(route, date);

        // Assert
        var expected = (int)(route.BasePrice * route.BaseFactor * 1.5M);
        Assert.Equal(expected, result);
    }
}

// Ejemplo mínimo de implementación para que el test compile
public class PriceCalculator : IPriceCalculator
{
    private readonly ITimeFactorProviderFactory _factorProvider;
    public PriceCalculator(ITimeFactorProviderFactory factorProvider)
    {
        _factorProvider = factorProvider;
    }

    public Task<int> CalculateFinalPriceAsync(FlightRoute flight, DateTime flightDate)
    {
        var factor = _factorProvider.GetFactor(flightDate);
        var price = (int)(flight.BasePrice * flight.BaseFactor * factor);
        return Task.FromResult(price);
    }
}