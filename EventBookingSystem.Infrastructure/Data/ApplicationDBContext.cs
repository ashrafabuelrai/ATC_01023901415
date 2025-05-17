using EventBookingSystem.Domain.Entities;
using EventBookingSystem.infrastructure.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventBookingSystem.infrastructure.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        protected ApplicationDBContext()
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventImage> Eventmages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ApplicaionUser
            modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.Id);
            modelBuilder.Entity<ApplicationUser>().HasKey(u=>u.Id);
            //Category
            modelBuilder.Entity<Category>()
            .HasMany(c => c.Events)
            .WithOne(e => e.Category)
            .HasForeignKey(e => e.CategoryId);
            modelBuilder.Entity<Category>().HasIndex(c => c.Id);
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            //Event
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Bookings)
                .WithOne(b => b.Event)
                .HasForeignKey(b => b.EventId);
            modelBuilder.Entity<Event>().HasIndex(e => e.Id);
            modelBuilder.Entity<Event>().HasKey(e => e.Id);
            //EventImage
            modelBuilder.Entity<EventImage>()
                .HasOne(e => e.Event)
                .WithMany(e => e.EventImages)
                .HasForeignKey(e => e.EventId);
            modelBuilder.Entity<EventImage>().HasIndex(e => e.Id);
            //Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.EventId);
            modelBuilder.Entity<Booking>().HasIndex(b => b.Id);
            modelBuilder.Entity<Booking>().HasKey(b => b.Id);
            modelBuilder.Entity<Booking>().HasIndex(b => b.UserId);
            base.OnModelCreating(modelBuilder);

        }
    }
}
