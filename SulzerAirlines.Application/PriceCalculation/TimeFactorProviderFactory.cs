using Microsoft.Extensions.Options;


namespace SulzerAirlines.Application.PriceCalculation;

public class TimeFactorProviderFactory : ITimeFactorProviderFactory
{
    private readonly TimeFactorRulesOptions _options;

    public TimeFactorProviderFactory(IOptions<TimeFactorRulesOptions> options)
    {
        _options = options.Value;
    }

    public decimal GetFactor(DateTime flightDate)
    {
        var hour = flightDate.Hour;

        foreach (var rule in _options.Rules)
        {
            if (hour >= rule.StartHour && hour < rule.EndHour)
            {
                return rule.Factor;
            }
        }

        return _options.DefaultFactor;
    }
}
