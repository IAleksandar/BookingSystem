namespace BookingSystem.Domain.Models
{
    using BookingSystem.Domain.Enums;

    public class Booking : BaseEntity
    {
        public DateTime BookingTime { get; set; }

        public string Destination { get; set; } = string.Empty;

        public string? DepartureAirport { get; set; } = string.Empty;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public BookingStatus Status { get; set; }

        public string BookingCode { get; set; } = string.Empty;

        public int SleepTime { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }
    }
}
