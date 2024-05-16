namespace BookingSystem.DataAccess
{
    using BookingSystem.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class BookingSystemDbContext : DbContext
    {
        public BookingSystemDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .Property(x => x.BookingTime)
                .IsRequired();

            modelBuilder.Entity<Booking>()
                .Property(x => x.Destination)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Booking>()
                 .Property(x => x.DepartureAirport)
                 .HasMaxLength(50);

            modelBuilder.Entity<Booking>()
                .Property(x => x.FromDate)
                .IsRequired();

            modelBuilder.Entity<Booking>()
                .Property(x => x.ToDate)
                .IsRequired();

            modelBuilder.Entity<Booking>()
                .Property(x => x.Status)
                .IsRequired();

            modelBuilder.Entity<Booking>()
                 .Property(x => x.BookingCode)
                 .HasMaxLength(6)
                 .IsRequired();

            modelBuilder.Entity<Booking>()
                 .Property(x => x.SleepTime)
                 .IsRequired();

            modelBuilder.Entity<Booking>()
                .HasOne(x => x.User)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<User>()
                .Property(x => x.FirstName)
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(x => x.LastName)
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(x => x.Username)
                .HasMaxLength(30);

            modelBuilder.Entity<User>()
                .Property(x => x.Role)
                .HasMaxLength(20);

            modelBuilder.Entity<User>()
                .Ignore(x => x.Age);
        }
    }
}
