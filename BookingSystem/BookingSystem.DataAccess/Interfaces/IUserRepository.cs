namespace BookingSystem.DataAccess.Interfaces
{
    using BookingSystem.Domain.Models;

    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);

        Task<User> LoginUser(string username, string password);

        Task Insert(User entity);

        Task Update(User entity);

        Task<User> GetById(int id);
    }
}
