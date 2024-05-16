namespace BookingSystem.Dtos
{
    public class SearchRequestDto
    {
        public string Destination { get; set; } = string.Empty;

        public string? DepartureAirport { get; set; } = string.Empty;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
