using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerAPI.Controllers
{
    /// <summary>
    /// API dedicated to the login process
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private ILogger<AuthController> _logger;

        public AuthController(IAuthRepository authRepository, ILogger<AuthController> logger)
        {
            _authRepository = authRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDTO loginRequest)
        {
            _logger.LogInformation(LogEvents.Login, "Login attempt {Username}", loginRequest.Username);
            try
            {
                var result = await _authRepository.LoginAsync(loginRequest);
                return Ok(result); // if login is successfull, a token is returned
            }
            catch (UserUnknownException)
            {
                _logger.LogInformation(LogEvents.Login, "Login attempt failed, user unkonwn: {Username}", loginRequest.Username);
                return Unauthorized(new LoginResultDTO()
                {
                    Success = false,
                    Message = "Invalid Username or Password."
                });
            }
            catch (PasswordNotMatchException)
            {
                _logger.LogInformation(LogEvents.Login, "Login attempt failed, password not match: {Username}", loginRequest.Username);
                return Unauthorized(new LoginResultDTO()
                {
                    Success = false,
                    Message = "Invalid Username or Password."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Login attempt failed, tickets exception: {Username} - {ex}", loginRequest.Username, ex);
                return Problem(ex.Message);
            }

        }



    }
}

