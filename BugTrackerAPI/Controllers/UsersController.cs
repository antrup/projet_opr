using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerAPI.Controllers
{
    /// <summary>
    /// API dedicated to users management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Return all users (id, username, email, roles) (admin only)
        /// </summary>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<ActionResult<ApiResult<UserInfoDTO>>> GetAllAsync(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null)
        {
            _logger.LogInformation(LogEvents.GetItems, "Getting users");
            try
            {
                var result = await _userRepository.GetAllAsync(pageIndex, pageSize, sortColumn, sortOrder).ConfigureAwait(false);
                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Getting users exception: {ex}", ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Return details of an user (id, username, email, roles)
        /// </summary>
        [Authorize(Roles = "RegisteredUser")]
        [HttpGet("{userID}")]
        public async Task<ActionResult<UserInfoDTO>> GetByIdAsync(string userID)
        {
            _logger.LogInformation(LogEvents.GetItem, "Getting user {userID}", userID);
            try
            {
                var result = await _userRepository.GetByIdAsync(userID).ConfigureAwait(false);
                return result;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(LogEvents.GetItemNotFound, "Getting user {userID} NOT FOUND: {ex}", userID, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Getting user {userID} exception: {ex}", userID, ex); ;
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Registration of a new user (admin only)
        /// </summary>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDTO registerRequest)
        {
            _logger.LogInformation(LogEvents.InsertItem, "Registering user {UserName}", registerRequest.UserName);
            try
            {
                await _userRepository.RegisterAsync(registerRequest).ConfigureAwait(false);
                return Ok(new ResultDTO()
                {
                    Success = true,
                    Message = "Registration successful",
                });
            }
            catch (UsernameAlreadyExistException)
            {
                _logger.LogInformation(LogEvents.OtherException, "Registration failed, {Username} already exist", registerRequest.UserName);
                return Ok(new ResultDTO()
                {
                    Success = false,
                    Message = "Username already exist"
                });
            }
            catch (EmailAlreadyExistException)
            {
                _logger.LogInformation(LogEvents.OtherException, "Registration failed, {Email} already exist", registerRequest.Email);
                return Ok(new ResultDTO()
                {
                    Success = false,
                    Message = "Username already exist"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Registration failed exception: {ex}", ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Delete an user (admin only)
        /// </summary>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{userID}")]
        public async Task<IActionResult> DeleteAsync(string userID)
        {
            _logger.LogInformation(LogEvents.DeleteItem, "Deleting user {userID}", userID);
            try
            {
                await _userRepository.DeleteAsync(userID);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(LogEvents.UpdateItemNotFound, "Deleting auser {userID} NOT FOUND: {ex}", userID, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Deleting application {userID} exception: {ex}", userID, ex);
                return Problem(ex.Message);
            }
        }
    }
}
