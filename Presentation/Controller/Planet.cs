using Microsoft.AspNetCore.Mvc;
using Infrastructure.Interface;

namespace Presentation.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class Planet : ControllerBase
    {
        private readonly ILogger<Planet> _logger;
        private readonly IRepo<Domain.Model.Planet> _repo;

        public Planet(ILogger<Planet> logger, IRepo<Domain.Model.Planet> repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet("GetAll", Name = "GetAll")]
        public async Task<IActionResult> GetPlanets()
        {
            try
            {
                var results = await _repo.GetAllAsync(shallow: true);
                return !results.Any() ? NotFound() : Ok(results);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in controller action" + nameof(Planet) + nameof(Get) + " at" + DateTime.Now.ToString() + " with message:" + e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("Get", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _repo.GetByIdAsync(id);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in controller action" + nameof(Planet) + nameof(Get) + " at" + DateTime.Now.ToString() + " with message:" + e.Message);
                return StatusCode(500);
            }
        }
    }
}