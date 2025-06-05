using System.ComponentModel.DataAnnotations;

namespace SulzerAirlines.Domain.Models;

/// <summary>
/// representa una ruta aérea entre dos ciudades. 
/// ATTENCION: Se se uso como Dominio de aplicacion y como DTO en la capa de la API para simplificar sin necesidad de mappers
/// </summary>
public class FlightRoute
{
    public City From { get; set; } //ciudad de origen, se podria tipar en una clase "Ciudad" con atributos "Pais", "Aeropuero", etc.                                   
    public City To { get; set; } // idem ciudad de destino.
    public decimal Distance { get; set; }
    public decimal BaseFactor { get; set; } // Factor de precio base por distancia se podria tipar en alguna clase de dominio "Factor" 
    public decimal BasePrice { get; set; } // Precio base del vuelo, se podria tipar en alguna clase "Precio", con su respectiva "Moneda
    private int _seatsLeft; // Asientos disponibles, se podria tipar en una clase coleccion de "Asientos", atibutos con cantidad y "Categoria" (Primera, Ejecutiva, Economica)
                            
    public int SeatsLeft
    {
        get => _seatsLeft;
        private set
        {
            if (value < 0) throw new InvalidOperationException("SeatsLeft debe ser positivo");
            _seatsLeft = value;
        }
    }

    public FlightRoute(City from, City to, decimal distance, decimal baseFactor, int seatsLeft, decimal basePrice)
    {
        From = from;
        To = to;
        Distance = distance;
        BasePrice = basePrice;
        _seatsLeft = seatsLeft;
        BaseFactor = baseFactor;
    }

    public bool HasEnoughSeats(int requestedSeats) => _seatsLeft >= requestedSeats;

    public void ReserveSeats(int seats)
    {
        if (seats <= 0)
            throw new ArgumentException("la cantidad de asientos de ver ser mayor a cero");
        if (SeatsLeft < seats)
            throw new InvalidOperationException("No hay asientos disponibles");

        SeatsLeft -= seats;
    }
}

/// <summary>
/// representa opciones de ruta dentre dos ciudades
/// </summary>
public class RouteOption
{
    public List<FlightRoute> Flights { get; }
    public int TotalPrice { get; }
    public RouteOption(List<FlightRoute> flights, int totalPrice)
    {
        Flights = flights;
        TotalPrice = totalPrice;
    }
}

/// <summary>
/// representa resultado de mejore horarios para volar entre dos ciudades
/// </summary>
public class BestTimeToFlyResult
{
    public TimeOnly Time { get; set; }
    public int Price { get; set; }
}