namespace BookingSystem.Helpers
{
    using BookingSystem.DataAccess;
    using BookingSystem.DataAccess.Implementations;
    using BookingSystem.DataAccess.Interfaces;
    using BookingSystem.Services.Implementations;
    using BookingSystem.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookingSystemDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void InjectRepositories(IServiceCollection services)
        {
            services.AddTransient<IManagerRepository, ManagerRepository>();
            services.AddTransient<IUserRepository, UsersRepository>();
        }

        public static void InjectServices(IServiceCollection services)
        {
            services.AddTransient<IManagerService, ManagerService>();
            services.AddTransient<IUsersService, UsersService>();
        }
    }
}
