using SulzerAirlines.Application.PriceCalculation;
using SulzerAirlines.Application.Services.Interfaces;
using SulzerAirlines.Domain.Interfaces;
using SulzerAirlines.Domain.Models;
using SulzerAirlines.Infrastructure.Repositories;
using System.Collections.Generic;
namespace SulzerAirlines.Application.Services;

public class BookingService : IBookingService
{
    private readonly IFlightRepository _flightRepository;
    private readonly IPriceCalculator _priceCalculator;

    public BookingService(IFlightRepository repository, IPriceCalculator priceCalculator)
    {
        _flightRepository = repository;
        _priceCalculator = priceCalculator;
    }
        
    // Reserva un vuelo a partir de la ruta y fecha/hora indicada
    public async Task<BookingResult> BookFlightAsync(City From, City To, DateTime FlightDateTime, decimal BPrice, int seats)
    {
        var FlightRoute = await _flightRepository.GetRouteAsync(From, To, BPrice);

        if (FlightRoute == null)
        {
            return new BookingResult(
                success: false,
                message: "No se encontro ruta para el origen-destino y precio  especificado",
                totalPrice: 0
            );
        }

        if(!FlightRoute.HasEnoughSeats(seats))
        {
            return new BookingResult(
                success: false,
                message: "No hay suficientes asientos disponibles",
                 totalPrice: 0
            );
        }
        else
        {
            FlightRoute.ReserveSeats(seats);
            var price = await _priceCalculator.CalculateFinalPriceAsync(FlightRoute, FlightDateTime);
            return new BookingResult(
                success: true,
                message: "Reserva realizada con éxito",
                totalPrice: (int)price*seats
            );
        }
    }   

   

    
}

