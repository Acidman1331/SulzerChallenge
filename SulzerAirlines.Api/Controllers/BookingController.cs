using SulzerAirlines.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SulzerAirlines.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    /// <summary>
    ///   reserva un vuelo a partir de la ruta y hora indicada
    /// </summary>
    [HttpPost("book")]
    public async Task<IActionResult> BookFlightAsync([FromQuery] string from, [FromQuery] string to, [FromQuery] string time, decimal BPrice, int seats)
    {
        
        if (!TimeOnly.TryParse(time, out var parsedTime))
            return BadRequest("Invalid time format. Use HH:mm."); //para evitar problemas con la hora de la reserva y simplifar sin uso dee BookingRequest

        var result = await _bookingService.BookFlightAsync(new City(from), new City(to), DateTime.Today.Add(parsedTime.ToTimeSpan()), BPrice, seats);

        if (!result.Success)
            return Conflict(result.Message);
        return Ok(result);
    }

}