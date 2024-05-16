namespace BookingSystem.Mappers
{
    using BookingSystem.Domain.Enums;
    using BookingSystem.Domain.Models;
    using BookingSystem.Dtos;

    public static class BookingMappers
    {
        public static Booking FromBookRequestDtoToBooking(this BookRequestDto bookRequestDto, string bookingCode)
        {
            return new Booking()
            {
                BookingTime = DateTime.Now,
                DepartureAirport = bookRequestDto.SearchRequest.DepartureAirport,
                Destination = bookRequestDto.SearchRequest.Destination,
                FromDate = bookRequestDto.SearchRequest.FromDate,
                ToDate = bookRequestDto.SearchRequest.ToDate,
                Status = BookingStatus.Pending,
                BookingCode = bookingCode,
                SleepTime = bookRequestDto.SleepTime
            };
        }

        public static BookResponseDto FromBookingToBookResponseDto(this Booking booking)
        {
            return new BookResponseDto()
            {
                BookingTime = DateTime.Now,
                BookingCode = booking.BookingCode
            };
        }
    }
}
