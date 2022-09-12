using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugTrackerAPI.Controllers
{
    /// <summary>
    /// API dedicated to providing stats regarding tickets
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {

        private readonly IStatsRepository _statsRepository;
        private readonly ILogger<StatsController> _logger;

        public StatsController(IStatsRepository statsRepository, ILogger<StatsController> logger)
        {
            _statsRepository = statsRepository;
            _logger = logger;
        }

        /// <summary>
        /// Return stats regarding tickets
        /// </summary>
        [Authorize(Roles = "RegisteredUser")]
        [HttpGet]
        public async Task<ActionResult<StatsDTO>> Getasync()
        {
            _logger.LogInformation(LogEvents.GetItems, "Getting stats");
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var result = await _statsRepository.Getasync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Getting stats exception: {ex}", ex);
                return Problem(ex.Message);
            }
        }


    }
}
