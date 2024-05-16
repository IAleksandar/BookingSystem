namespace BookingSystem.Services.Implementations
{
    using BookingSystem.DataAccess.Interfaces;
    using BookingSystem.Domain.Enums;
    using BookingSystem.Domain.Models;
    using BookingSystem.Dtos;
    using BookingSystem.Mappers;
    using BookingSystem.Services.Interfaces;
    using BookingSystem.Utils;

    public class ManagerService : IManagerService
    {
        private readonly ApiClient _apiClient;
        private readonly IManagerRepository _managerRepository;
        private readonly IUserRepository _userRepository;

        public ManagerService(IManagerRepository managerRepository, IUserRepository userRepository)
        {
            _apiClient = new ApiClient();
            _managerRepository = managerRepository;
            _userRepository = userRepository;
        }

        public async Task<BookResponseDto> Book(BookRequestDto bookRequestDto)
        {
            if (bookRequestDto == null)
            {
                throw new Exception("Invalid input");
            }

            string bookingCode = GenerateBookingCode();
            Booking newBooking = bookRequestDto.FromBookRequestDtoToBooking(bookingCode);
            User user = await _userRepository.GetById(bookRequestDto.UserId);
            Booking bookingInfo = await _managerRepository.CreateBooking(newBooking);
            user.Bookings.Add(bookingInfo);
            await _userRepository.Update(user);

            return bookingInfo.FromBookingToBookResponseDto();
        }

        public async Task<CheckStatusResponseDto> CheckStatus(CheckStatusRequestDto checkStatusRequestDto)
        {
            Booking bookingDB = await _managerRepository.GetBooking(checkStatusRequestDto.BookingCode);

            DateTime completionTime = bookingDB.BookingTime.AddSeconds(bookingDB.SleepTime);

            if (!string.IsNullOrEmpty(bookingDB.DepartureAirport))
            {
                await _managerRepository.ChangeBookingStatus(BookingStatus.Success, bookingDB.Id);
            }

            DateTime currentDate = DateTime.Now;

            DateTime futureDate = currentDate.AddDays(45);

            if (bookingDB.FromDate > currentDate && bookingDB.FromDate <= futureDate)
            {
                await _managerRepository.ChangeBookingStatus(BookingStatus.Failed, bookingDB.Id);
            }

            if (bookingDB.BookingTime > completionTime)
            {
                await _managerRepository.ChangeBookingStatus(BookingStatus.Success, bookingDB.Id);
            }

            return new CheckStatusResponseDto()
            {
                Status = bookingDB.Status
            };
        }

        public async Task<SearchResponseDto> Search(SearchRequestDto searchRequestDto)
        {
            ValidateState(searchRequestDto);

            if (string.IsNullOrEmpty(searchRequestDto.DepartureAirport))
            {
                List<OptionDto> searchResponseDto = await _apiClient.SearchFlights(searchRequestDto);

                SearchResponseDto searchResult = new SearchResponseDto()
                {
                    Options = searchResponseDto
                };

                return searchResult;
            }

            DateTime currentDate = DateTime.Now;

            DateTime futureDate = currentDate.AddDays(45);

            if (searchRequestDto.FromDate > currentDate && searchRequestDto.FromDate <= futureDate)
            {
                List<OptionDto> searchResponseDto = await _apiClient.SearchHotels(searchRequestDto);

                SearchResponseDto searchResult = new SearchResponseDto()
                {
                    Options = searchResponseDto
                };

                return searchResult;
            }

            List<OptionDto> searchResponseDtoHotels = await _apiClient.SearchHotels(searchRequestDto);
            List<OptionDto> searchResponseDtoFlights = await _apiClient.SearchFlights(searchRequestDto);

            List<OptionDto> combinedSearchResult = new List<OptionDto>(searchResponseDtoHotels);
            combinedSearchResult.AddRange(searchResponseDtoFlights);

            SearchResponseDto result = new SearchResponseDto()
            {
                Options = combinedSearchResult
            };

            return result;
        }

        private string GenerateBookingCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var code = new char[6];
            for (int i = 0; i < 6; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }
            return new string(code);
        }

        private void ValidateState(SearchRequestDto searchRequestDto)
        {
            if (searchRequestDto == null)
            {
                throw new Exception("Invalid input");
            }

            if (string.IsNullOrEmpty(searchRequestDto.Destination))
            {
                throw new Exception("Destination is required");
            }

            if (searchRequestDto.FromDate == null)
            {
                throw new Exception("From Date is required");
            }

            if (searchRequestDto.ToDate == null)
            {
                throw new Exception("To Date is required");
            }
        }
    }
}
