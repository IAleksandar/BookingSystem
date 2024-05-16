namespace BookingSystem.DataAccess.Implementations
{
    using BookingSystem.DataAccess.Interfaces;
    using BookingSystem.Domain.Enums;
    using BookingSystem.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class ManagerRepository : IManagerRepository
    {
        private BookingSystemDbContext _bookingSystemDbContext;

        public ManagerRepository(BookingSystemDbContext bookingSystemDbContext)
        {
            this._bookingSystemDbContext = bookingSystemDbContext;
        }

        public async Task ChangeBookingStatus(BookingStatus bookingStatus, int bookingId)
        {
            Booking bookingDb = await GetBooking(bookingId);

            if (bookingDb == null)
            {
                throw new Exception("Booking not found");
            }

            bookingDb.Status = bookingStatus;

            await _bookingSystemDbContext.SaveChangesAsync();
        }

        public async Task<Booking> CreateBooking(Booking booking)
        {
            await _bookingSystemDbContext.Bookings.AddAsync(booking);
            await _bookingSystemDbContext.SaveChangesAsync();

            int id = _bookingSystemDbContext.Bookings.LastOrDefaultAsync().Id;

            return await GetBooking(id);
        }

        public async Task<Booking> GetBooking(string bookingCode)
        {
            Booking bookingDb = await _bookingSystemDbContext.Bookings.FirstOrDefaultAsync(x => x.BookingCode == bookingCode);

            if (bookingDb == null)
            {
                throw new Exception($"Booking with booking code {bookingCode} not found.");
            }

            return bookingDb;
        }

        private async Task<Booking> GetBooking(int id)
        {
            Booking bookingDb = await _bookingSystemDbContext.Bookings.FirstOrDefaultAsync(x => x.Id == id);

            if (bookingDb == null)
            {
                throw new Exception($"Booking with id {id} not found.");
            }

            return bookingDb;
        }
    }
}
