public class GetAvailableFlightsQuery
{
    public City From { get; }
    public City To { get; }

    public GetAvailableFlightsQuery(City from, City to)
    {
        From = from;
        To = to;
    }
}