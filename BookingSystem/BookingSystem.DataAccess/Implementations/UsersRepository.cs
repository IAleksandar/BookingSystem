namespace BookingSystem.DataAccess.Implementations
{
    using BookingSystem.DataAccess.Interfaces;
    using BookingSystem.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class UsersRepository : IUserRepository
    {
        private BookingSystemDbContext _bookingSystemDbContext;
        public UsersRepository(BookingSystemDbContext bookingSystemDbContext)
        {
            _bookingSystemDbContext = bookingSystemDbContext;
        }

        public async Task<User> GetById(int id)
        {
            User userDb = await _bookingSystemDbContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if(userDb == null)
            {
                throw new Exception("User not found");
            }

            return userDb;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Invalid input");
            }

            User userDb = await _bookingSystemDbContext.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());

            if (userDb == null)
            {
                throw new Exception("User not found");
            }

            return userDb;
        }

        public async Task Insert(User entity)
        {
            if(entity == null)
            {
                throw new Exception("Invalid input");
            }

            _bookingSystemDbContext.Users.AddAsync(entity);
            _bookingSystemDbContext.SaveChangesAsync();
        }

        public async Task<User> LoginUser(string username, string password)
        {
            User userDb = await _bookingSystemDbContext.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower()
            && x.Password == password);

            if (userDb == null)
            {
                throw new Exception("User not found");
            }

            return userDb;
        }

        public async Task Update(User entity)
        {
            if(entity == null)
            {
                throw new Exception("Invalid input");
            }

            User userDb = await GetUser(entity.Id);
            
            userDb.Address = entity.Address;
            userDb.Password = entity.Password;
            userDb.Username = entity.Username;
            userDb.Role = entity.Role;
            userDb.LastName = entity.LastName;
            userDb.FirstName = entity.FirstName;
            userDb.Bookings = entity.Bookings;
            userDb.Age = entity.Age;

            _bookingSystemDbContext.Update(userDb);
            await _bookingSystemDbContext.SaveChangesAsync();
        }

        private async Task<User> GetUser(int id)
        {
            User userDb = await _bookingSystemDbContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (userDb == null)
            {
                throw new Exception("User not found");
            }

            return userDb;
        }
    }
}
