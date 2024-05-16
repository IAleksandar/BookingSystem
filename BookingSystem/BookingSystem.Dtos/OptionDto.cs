namespace BookingSystem.Dtos
{
    public class OptionDto
    {
        public string OptionCode { get; set; } = string.Empty;

        public string HotelCode { get; set; } = string.Empty;

        public string FlightCode { get; set; } = string.Empty;

        public string ArrivalAirport { get; set; } = string.Empty;

        public double Price { get; set; }
    }
}
