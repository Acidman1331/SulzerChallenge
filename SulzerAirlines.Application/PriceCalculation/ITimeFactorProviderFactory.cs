namespace SulzerAirlines.Application.PriceCalculation;

public interface ITimeFactorProviderFactory
{
    decimal GetFactor(DateTime flightDate);
}