using SulzerAirlines.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SulzerAirlines.Application.PriceCalculation;

public class PriceCalculator : IPriceCalculator
{
    private readonly ITimeFactorProviderFactory _timeFactorFactory;

    public PriceCalculator(ITimeFactorProviderFactory timeFactorFactory)
    {
        _timeFactorFactory = timeFactorFactory;
    }

    public Task<int> CalculateFinalPriceAsync(FlightRoute Flight, DateTime FlightDate)
    {
        var timeFactor = _timeFactorFactory.GetFactor(FlightDate);

        var finalPrice = (Flight.BasePrice + (Flight.BaseFactor * Flight.Distance) * timeFactor);
        return Task.FromResult((int)finalPrice);
    }
}