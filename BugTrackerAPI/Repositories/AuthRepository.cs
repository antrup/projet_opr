using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace BugTrackerAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<BugTrackerUser> _userManager;
        private readonly JwtHandler _jwtHandler;

        // UserManager (identity) and JwtHandler services injections
        public AuthRepository(
           UserManager<BugTrackerUser> userManager,
           JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        public async Task<LoginResultDTO> LoginAsync(LoginDTO loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Username).ConfigureAwait(false);

            if (user == null)
                throw new UserUnknownException();

            if (!await _userManager.CheckPasswordAsync(user, loginRequest.Password).ConfigureAwait(false))
                throw new PasswordNotMatchException();

            // if user exist and password match, generate jwt token
            var secToken = await _jwtHandler.GetTokenAsync(user).ConfigureAwait(false);
            var jwt = new JwtSecurityTokenHandler().WriteToken(secToken);

            // get roles assigned to user
            var roles = (IEnumerable<string>)await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            return (new LoginResultDTO()
            {
                Success = true,
                Message = "Login successful",
                Token = jwt,
                UserName = user.UserName,
                Id = user.Id,
                Roles = roles
            });
        }
    }

    // Exception dedicated to user not found cases
    public class UserUnknownException : Exception
    {
        public UserUnknownException() { }

        public UserUnknownException(string message) : base(message) { }

        public UserUnknownException(string message, Exception innerException) : base(message, innerException) { }
    }

    // Exception dedicated to password not matching cases
    public class PasswordNotMatchException : Exception
    {
        public PasswordNotMatchException() { }

        public PasswordNotMatchException(string message) : base(message) { }

        public PasswordNotMatchException(string message, Exception innerException) : base(message, innerException) { }
    }
}
