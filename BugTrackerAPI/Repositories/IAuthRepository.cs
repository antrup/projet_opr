using BugTrackerAPI.Data.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerAPI.Repositories
{
    public interface IAuthRepository
    {
        // Handle login request and return token and other infos (roles, username ...) if successfull
        Task<LoginResultDTO> LoginAsync(LoginDTO loginRequest);
    }
}
