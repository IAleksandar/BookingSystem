namespace BookingSystem.DataAccess.Interfaces
{
    using BookingSystem.Domain.Enums;
    using BookingSystem.Domain.Models;

    public interface IManagerRepository
    {
        Task<Booking> CreateBooking(Booking booking);

        Task ChangeBookingStatus(BookingStatus bookingStatus, int bookignId);

        Task<Booking> GetBooking(string bookingCode);
    }
}
