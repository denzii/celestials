
using Moq;
using Microsoft.EntityFrameworkCore;
using Domain.Model;
using Infrastructure.Persistence;
using Infrastructure.Service;

namespace CelestialsTest.Tests
{
    public class RepoTests
    {
        [Fact]
        public async Task GetByIdAsync_ReturnsEntity_WhenEntityExists()
        {
            // Arrange
            var entities = new List<Planet>
            {
                new Planet { Id = 1, Name = "Planet 1" },
                new Planet { Id = 2, Name = "Planet 2" },
                new Planet { Id = 3, Name = "Planet 3" }
            };

            var mockDbSet = new Mock<DbSet<Planet>>();
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.Provider).Returns(entities.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => new ValueTask<Planet>(entities.FirstOrDefault(p => (int)ids[0] == p.Id)));

            var mockDbContext = new Mock<AppDbContext>();
            mockDbContext.Setup(c => c.Set<Planet>()).Returns(mockDbSet.Object);

            var repo = new Repo<Planet>(mockDbContext.Object);

            // Act
            var entity = await repo.GetByIdAsync(2);

            // Assert
            Assert.NotNull(entity);
            Assert.Equal("Planet 2", entity.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity_WhenEntityDoesntExists()
        {
            // Arrange
            var entities = new List<Planet>
            {
                new Planet { Id = 1, Name = "Planet 1" },
                new Planet { Id = 2, Name = "Planet 2" },
                new Planet { Id = 3, Name = "Planet 3" }
            };

            var mockDbSet = new Mock<DbSet<Planet>>();
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.Provider).Returns(entities.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).Returns<object[]>(ids => new ValueTask<Planet>(entities.FirstOrDefault(p => (int)ids[0] == p.Id)));

            var mockDbContext = new Mock<AppDbContext>();
            mockDbContext.Setup(c => c.Set<Planet>()).Returns(mockDbSet.Object);

            var repo = new Repo<Planet>(mockDbContext.Object);

            // Act
            var entity = await repo.GetByIdAsync(5);

            // Assert
            Assert.Null(entity);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEntities_WhenShallowIsFalse()
        {
            // Arrange
            // Arrange
            var entities = new List<Planet>
            {
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
            };

            var mockDbSet = new Mock<DbSet<Planet>>();
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.Provider).Returns(entities.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Planet>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());
            
            mockDbSet.As<IAsyncEnumerable<Planet>>().Setup(m => m.GetAsyncEnumerator(CancellationToken.None)).Returns(new TestAsyncEnumerator<Planet>(entities.GetEnumerator()));

            var mockDbContext = new Mock<AppDbContext>();
            mockDbContext.Setup(c => c.Set<Planet>()).Returns(mockDbSet.Object);

            var repo = new Repo<Planet>(mockDbContext.Object);

            // Act
            var result = await repo.GetAllAsync(false);

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal(entities, result);
        }
    

   [Fact]
    public async Task GetAllAsync_ReturnsAllEntities_WhenShallowIsTrue()
    {
        // Arrange
            var entities = new List<Planet>
            {
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
            };

        var mockDbSet = new Mock<DbSet<Planet>>();
        mockDbSet.As<IQueryable<Planet>>().Setup(m => m.Provider).Returns(entities.AsQueryable().Provider);
        mockDbSet.As<IQueryable<Planet>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
        mockDbSet.As<IQueryable<Planet>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<Planet>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());

        mockDbSet.As<IAsyncEnumerable<Planet>>()
            .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
            .Returns(new TestAsyncEnumerator<Planet>(entities.GetEnumerator()));

        var mockDbContext = new Mock<AppDbContext>();
        mockDbContext.Setup(c => c.Set<Planet>()).Returns(mockDbSet.Object);

        var repo = new Repo<Planet>(mockDbContext.Object);

        // Act
        var result = await repo.GetAllAsync(true);

        // Assert
        Assert.Equal(3, result.Count());

        var expectedEntities = entities.Select(x => new BaseEntity { Id = x.Id, Name = x.Name }).ToList();

        for (int i = 0; i < expectedEntities.Count; i++)
        {
            Assert.Equal(expectedEntities[i].Id, ((BaseEntity)result.ElementAt(i)).Id);
            Assert.Equal(expectedEntities[i].Name, ((BaseEntity)result.ElementAt(i)).Name);
        }
    }  
        [Fact]
        public async Task AddAsync_AddsEntityToDatabase()
        {
            // Arrange
            var entity = new Planet { Id = 1, Name = "Planet 1" };

            var mockDbSet = new Mock<DbSet<Planet>>();
            mockDbSet.Setup(d => d.AddAsync(entity, default)).Verifiable();

            var mockDbContext = new Mock<AppDbContext>();
            mockDbContext.Setup(c => c.Set<Planet>()).Returns(mockDbSet.Object);
            mockDbContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var repo = new Repo<Planet>(mockDbContext.Object);

            // Act
            var result = await repo.AddAsync(entity);

            // Assert
            mockDbSet.Verify();
            mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task DeleteAsync_RemovesEntityFromDatabase()
        {
            // Arrange
            var entity = new Planet { Id = 1, Name = "Planet 1" };

            var mockDbSet = new Mock<DbSet<Planet>>();
            mockDbSet.Setup(d => d.Remove(entity)).Verifiable();

            var mockDbContext = new Mock<AppDbContext>();
            mockDbContext.Setup(c => c.Set<Planet>()).Returns(mockDbSet.Object);
            mockDbContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var repo = new Repo<Planet>(mockDbContext.Object);

            // Act
            var result = await repo.DeleteAsync(entity);

            // Assert
            mockDbSet.Verify();
            mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
            Assert.Equal(1, result);
        }
    }
}
