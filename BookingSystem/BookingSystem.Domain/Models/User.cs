namespace BookingSystem.Domain.Models
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<Booking> Bookings { get; set; }
        public int Age { get; set; }
        public string Role { get; set; }
    }
}
