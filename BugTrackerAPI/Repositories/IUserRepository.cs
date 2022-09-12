using BugTrackerAPI.Data.DTO;

namespace BugTrackerAPI.Repositories
{
    public interface IUserRepository
    {
        // Return all users with paging and sorting (as per parameters)
        Task<ApiResult<UserInfoDTO>> GetAllAsync(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null);

        // Return the details of user matching userID
        Task<UserInfoDTO> GetByIdAsync(string userID);
        
        // Register a new user as per data passed in parameter
        Task RegisterAsync(RegisterDTO registerRequest);

        // Delete user whose id is passed in parameter
        Task DeleteAsync(string userID);
    }
}
