using SulzerAirlines.Application.Services.Interfaces;
using SulzerAirlines.Domain.Models;

public class BookFlightCommandHandler
{
    private readonly IBookingService _bookingService;

    public BookFlightCommandHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<BookingResult> Handle(BookFlightCommand command)
    {
        return await _bookingService.BookFlightAsync(
            command.From,
            command.To,
            command.FlightDateTime,
            command.BPrice,
            command.Seats
        );
    }
}