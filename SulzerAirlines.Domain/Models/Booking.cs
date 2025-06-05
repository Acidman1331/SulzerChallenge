using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SulzerAirlines.Domain.Models;


/// <summary>
/// representa solicitud de una reserva de vuelo
/// </summary>
public class BookingRequest
{
    [Required]
    public City ?From { get; set; }

    [Required]
    public City ?To { get; set; }

    [Required]
    public string ?Time { get; set; } //se pasa la hora para simplificar

    /* al no tener idenficiador de ruta, se obtiene en el repo de vuelos por From, To y BasePrice (como "clave" compuesta).*/
    [Required] 
    [Range(1, (double)decimal.MaxValue, ErrorMessage = "Debe indica el precio del vuelo elegido")] 
    public decimal BasePrice { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Debe indica al menos 1 asienoto")]
    public int Seats { get; set; }

}

/// <summary>
/// representa resultado de una reserva de vuelo
/// </summary>
public class BookingResult
{
    public bool Success { get; }
    public string Message { get; }
    public int TotalPrice { get; }
    public DateTime FlightDateTime { get; set; } 

    public BookingResult(bool success, string message, int totalPrice = 0)
    {
        Success = success;
        Message = message;
        TotalPrice = totalPrice;
    }
}


