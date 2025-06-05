using SulzerAirlines.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SulzerAirlines.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class FlightController : ControllerBase
{
    private readonly IFlightService _flightService;

    public FlightController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    /// <summary>
    ///   obtiene la ruta más economico a partir de la hora indicada
    /// </summary>
    [HttpGet("cheapest")]
    public async Task<IActionResult> GetCheapestRoute([FromQuery] string from, [FromQuery] string to, [FromQuery] string time)
    {
        
        if (!TimeOnly.TryParse(time, out var parsedTime))
            return BadRequest("Invalid time format. Use HH:mm.");
             
        var result = await _flightService.GetCheapestRouteAsync(new City(from), new City(to), DateTime.Today.Add(parsedTime.ToTimeSpan()));
        
        if (!result.Any()) return NotFound("No route found.");

        return Ok(result);
    }

    /// <summary>
    /// .Obtiene horario más convieniente para una ruta 
    /// </summary>
    [HttpGet("besttime")]
    public async Task<IActionResult> GetBestTime([FromQuery] string from, [FromQuery] string to)
    {
        
        var result = await _flightService.GetBestTimeToFlyAsync( new City(from), new City(to));
        if (!result.Any()) return NotFound("No route found.");
        return Ok(result);
    }

    /// <summary>
    /// Obtiene vuelos ida y vuelta con al menos 1 conexión
    /// </summary>
    [HttpGet("roundtrip")]
    public async Task<IActionResult> GetRoundTrip([FromQuery] string from, [FromQuery] string to, [FromQuery] int maxconn)
    {
        
        var result = await _flightService.GetRoundTripAsync(new City(from), new City(to),maxconn );
        if (!result.Any()) return NotFound("No round trips found..");
        return Ok(result);
    }
}