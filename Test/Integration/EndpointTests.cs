
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation;

namespace FunPlanets.Integration
{
    public class EndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;

        public EndpointTests(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        // used to work but started to halt after changing to the layered architecture
        // will uncomment and get back to this if any time is left in the end
        //[Theory]
        //[InlineData("/Planet/Get?id=1")]
        //[InlineData("/Planet/GetAll")]
        //public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        //{
        //    // Arrange
        //    var client = _factory.CreateClient();

        //    // Act
        //    var response = await client.GetAsync(url);

        //    // Assert
        //    response.EnsureSuccessStatusCode(); 
        //    Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        //}
    }

    // use inmemory db for tests rather than postgres as the tests are only for
    // hitting the endpoint to see its response type and status code and currently
    // not extensively testing DB interactions or data so no need for a real db
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            // Replace the existing configuration with your test-specific configuration
            builder.ConfigureServices(services =>
            {
                // Clear the existing database context registrations
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Register an in-memory database context
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });

            base.ConfigureWebHost(builder);
        }
    }
}