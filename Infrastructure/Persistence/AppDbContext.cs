using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Planet> Planets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Planet>().HasKey(p => p.Id);
            modelBuilder.Entity<Planet>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Planet>().Property(p => p.PhotoUrl).IsRequired();
            modelBuilder.Entity<Planet>().Property(p => p.DistanceFromSunKilometers).IsRequired();
            modelBuilder.Entity<Planet>().Property(p => p.MassTonnes).IsRequired();
            modelBuilder.Entity<Planet>().Property(p => p.DiameterKilometers).IsRequired();
            modelBuilder.Entity<Planet>().Property(p => p.LengthOfDayHours).IsRequired();
                
            modelBuilder.Entity<Planet>().HasData(
                    new Planet
                    {
                        Id = 1,
                        Name = "Mercury",
                        DiameterKilometers = 4879,
                        DistanceFromSunKilometers = 57910000,
                        LengthOfDayHours = 4222.6,
                        MassTonnes = 330.2,
                        PhotoUrl = "https://dummyimage.com/300x300/000000/ffffff"
                    },
                    new Planet
                    {
                        Id = 2,
                        Name = "Venus",
                        DiameterKilometers = 12104,
                        DistanceFromSunKilometers = 108200000,
                        LengthOfDayHours = 2802,
                        MassTonnes = 4868.5,
                        PhotoUrl = "https://dummyimage.com/300x300/000000/ffffff"
                    },
                    new Planet
                    {
                        Id = 3,
                        Name = "Earth",
                        DiameterKilometers = 12742,
                        DistanceFromSunKilometers = 149600000,
                        LengthOfDayHours = 24,
                        MassTonnes = 5973.6,
                        PhotoUrl = "https://dummyimage.com/300x300/000000/ffffff"
                    },
                    new Planet
                    {
                        Id = 4,
                        Name = "Mars",
                        DiameterKilometers = 6779,
                        DistanceFromSunKilometers = 227900000,
                        LengthOfDayHours = 24.7,
                        MassTonnes = 641.71,
                        PhotoUrl = "https://dummyimage.com/300x300/000000/ffffff"
                    },
                    new Planet
                    {
                        Id = 5,
                        Name = "Jupiter",
                        DiameterKilometers = 139820,
                        DistanceFromSunKilometers = 778500000,
                        LengthOfDayHours = 9.9,
                        MassTonnes = 1898600,
                        PhotoUrl = "https://dummyimage.com/300x300/000000/ffffff"
                    }
                );
        }
    }
}