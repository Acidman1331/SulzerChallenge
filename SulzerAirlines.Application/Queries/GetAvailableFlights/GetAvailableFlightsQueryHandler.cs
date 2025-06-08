using SulzerAirlines.Application.Services.Interfaces;
using SulzerAirlines.Domain.Models;

public class GetAvailableFlightsQueryHandler
{
    private readonly IFlightService _flightService;

    public GetAvailableFlightsQueryHandler(IFlightService flightService)
    {
        _flightService = flightService;
    }

    public async Task<IReadOnlyList<FlightRoute>> Handle(GetAvailableFlightsQuery query)
    {
        return await _flightService.GetRoutesAsync(query.From, query.To);
    }
}