

using SulzerAirlines.Domain.Models;

namespace SulzerAirlines.Application.PriceCalculation;

public interface IPriceCalculator
{
    Task<int> CalculateFinalPriceAsync(FlightRoute Flight, DateTime FlightDate);
}


