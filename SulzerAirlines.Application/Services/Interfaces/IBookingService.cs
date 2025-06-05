using SulzerAirlines.Domain.Models;

namespace SulzerAirlines.Application.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResult> BookFlightAsync(City From, City To, DateTime FlightDateTime , decimal BPrice, int seats);
}

