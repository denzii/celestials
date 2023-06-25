
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Interface;

namespace CelestialsTest.Unit
{
    public class PlanetControllerTests
    {
        private readonly Mock<ILogger<Presentation.Controller.Planet>> _loggerMock;
        private readonly Mock<IRepo<Domain.Model.Planet>> _repoMock;
        private readonly Presentation.Controller.Planet _sut;

        public PlanetControllerTests()
        {
            _loggerMock = new Mock<ILogger<Presentation.Controller.Planet>>();
            _repoMock = new Mock<IRepo<Domain.Model.Planet>>();
            _sut = new Presentation.Controller.Planet(_loggerMock.Object, _repoMock.Object);
        }

        // Tests for GetPlanets action

        [Fact]
        public async Task GetPlanets_ReturnsSuccess_WhenDataExists()
        {
            // Arrange
            var planets = new List<Domain.Model.Planet>
            {
                new Domain.Model.Planet
                {
                    PhotoUrl = "http://example.com/planet1.jpg",
                    DistanceFromSunKilometers = 1000000,
                    MassTonnes = 500000,
                    DiameterKilometers = 8000,
                    LengthOfDayHours = 24
                },
                new Domain.Model.Planet
                {
                    PhotoUrl = "http://example.com/planet2.jpg",
                    DistanceFromSunKilometers = 2000000,
                    MassTonnes = 1000000,
                    DiameterKilometers = 10000,
                    LengthOfDayHours = 30
                }
            };
            _repoMock.Setup(r => r.GetAllAsync(It.IsAny<bool>())).ReturnsAsync(planets);

            // Act
            var result = await _sut.GetPlanets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Domain.Model.Planet>>(okResult.Value);
            Assert.Equal(2, model.Count()); // Asserting the count of returned planets
            // Additional assertions as per your requirements
        }

        [Fact]
        public async Task GetPlanets_ReturnsNotFound_WhenNoDataExists()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync(It.IsAny<bool>())).ReturnsAsync(new List<Domain.Model.Planet>());

            // Act
            var result = await _sut.GetPlanets();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetPlanets_ReturnsStatusCode500_WhenExceptionOccurs()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync(It.IsAny<bool>())).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetPlanets();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        // Tests for Get action

        [Fact]
        public async Task Get_ReturnsSuccess_WhenDataExists()
        {
            // Arrange
            var planetId = 1;
            var planet = new Domain.Model.Planet
            {
                PhotoUrl = "http://example.com/planet1.jpg",
                DistanceFromSunKilometers = 1000000,
                MassTonnes = 500000,
                DiameterKilometers = 8000,
                LengthOfDayHours = 24
            };
            _repoMock.Setup(r => r.GetByIdAsync(planetId)).ReturnsAsync(planet);

            // Act
            var result = await _sut.Get(planetId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Domain.Model.Planet>(okResult.Value);
            // Additional assertions as per your requirements
            Assert.Equal(planet.PhotoUrl, model.PhotoUrl);
            Assert.Equal(planet.DistanceFromSunKilometers, model.DistanceFromSunKilometers);
            // Assert other properties
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenDataDoesNotExist()
        {
            // Arrange
            var planetId = 1;
            _repoMock.Setup(r => r.GetByIdAsync(planetId)).ReturnsAsync(null as Domain.Model.Planet);

            // Act
            var result = await _sut.Get(planetId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsStatusCode500_WhenExceptionOccurs()
        {
            // Arrange
            var planetId = 1;
            _repoMock.Setup(r => r.GetByIdAsync(planetId)).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.Get(planetId);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
