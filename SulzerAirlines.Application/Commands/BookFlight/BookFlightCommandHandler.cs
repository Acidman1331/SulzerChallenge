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
        var result = await _bookingService.BookFlightAsync(
            command.From,
            command.To,
            command.FlightDateTime,
            command.BPrice,
            command.Seats
        );

        if (result.Success)
        {
            // Aquí publicarías el evento a Kafka
            // await _eventBus.PublishAsync(new BookingCreatedEvent(...));
        }

        return result;
    }
}