using BugTrackerAPI.Data;
using BugTrackerAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerAPI.Controllers
{
    /// <summary>
    /// API for deployment purpose (creation of default roles and default admin user)
    /// </summary>
    /// <remarks>
    /// Default admin password is set in Secrets file
    /// </remarks>
    //[Authorize(Roles = "Administrator")] to activate after deployment
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeedController : Controller
    {
        private readonly ISeedRepository _seedRepository;
        private ILogger<SeedController> _logger;

        public SeedController(ISeedRepository seedRepository, ILogger<SeedController> logger)
        {
            _logger = logger;
            _seedRepository = seedRepository;
        }

        [HttpGet]
        public async Task<ActionResult> CreateDefaultRolesAsync()
        {
            _logger.LogInformation(LogEvents.InsertItem, "Seeding default roles");
            try
            {
                await _seedRepository.CreateDefaultRolesAsync().ConfigureAwait(false);
                return Ok(new JsonResult(new
                {
                    Result = "ok"
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Seeding default admin failed: {ex}", ex);
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CreateDefaultAdminAsync()
        {
            _logger.LogInformation(LogEvents.InsertItem, "Seeding default admin");
            try
            {
                await _seedRepository.CreateDefaultAdminAsync().ConfigureAwait(false);
                return Ok(new JsonResult(new
                {
                    Result = "ok"
                }));
            }
            catch (UsernameAlreadyExistException)
            {
                _logger.LogWarning(LogEvents.OtherException, "Seeding default admin failed, user already exist");
                return Ok(new JsonResult(new
                {
                    Result = "default admin already exist"
                }));
            }
            catch (EmailAlreadyExistException)
            {
                _logger.LogWarning(LogEvents.OtherException, "Seeding default admin failed, email already exist");
                return Ok(new JsonResult(new
                {
                    Result = "default admin email already exist"
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Seeding default admin failed: {ex}", ex);
                return Problem(ex.Message);
            }
        }

    }
}
