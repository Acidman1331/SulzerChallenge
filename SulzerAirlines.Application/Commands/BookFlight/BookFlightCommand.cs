public class BookFlightCommand
{
    public City From { get; }
    public City To { get; }
    public DateTime FlightDateTime { get; }
    public decimal BPrice { get; }
    public int Seats { get; }

    public BookFlightCommand(City from, City to, DateTime flightDateTime, decimal bPrice, int seats)
    {
        From = from;
        To = to;
        FlightDateTime = flightDateTime;
        BPrice = bPrice;
        Seats = seats;
    }
}