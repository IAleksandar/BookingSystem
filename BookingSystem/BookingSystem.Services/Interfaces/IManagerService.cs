namespace BookingSystem.Services.Interfaces
{
    using BookingSystem.Dtos;

    public interface IManagerService
    {
        Task<SearchResponseDto> Search(SearchRequestDto searchRequestDto);

        Task<BookResponseDto> Book(BookRequestDto bookRequestDto);

        Task<CheckStatusResponseDto> CheckStatus(CheckStatusRequestDto checkStatusRequestDto);
    }
}
