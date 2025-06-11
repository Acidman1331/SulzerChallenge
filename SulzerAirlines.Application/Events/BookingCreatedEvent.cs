public class BookingCreatedEvent
{
    public City From { get; }
    public City To { get; }
    public DateTime FlightDateTime { get; }
    public int Seats { get; }
    public int TotalPrice { get; }

    public BookingCreatedEvent(City from, City to, DateTime flightDateTime, int seats, int totalPrice)
    {
        From = from;
        To = to;
        FlightDateTime = flightDateTime;
        Seats = seats;
        TotalPrice = totalPrice;
    }
}