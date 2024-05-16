namespace BookingSystem.Services.Interfaces
{
    using BookingSystem.Dtos;

    public interface IUsersService
    {
        Task Register(RegisterUserDto registerUserDto);

        Task<string> Login(LoginDto loginDto);
    }
}
