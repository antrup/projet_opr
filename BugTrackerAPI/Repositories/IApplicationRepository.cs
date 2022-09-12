using BugTrackerAPI.Data.DTO;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace BugTrackerAPI.Repositories
{
    public interface IApplicationRepository
    {
        // Return all applications with paging and sorting (as per parameters)
        Task<ApiResult<ApplicationDTO>> GetAllAsync(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null);

        // Return the details of application matching application ID
        Task<ApplicationDTO> GetByIdAsync(int applicationID);

        // Insert application passed in parameter into application table of DB
        Task InsertAsync(NewApplicationDTO application);

        // Delete application whose id is passed in parameter from application table of DB
        Task DeleteAsync(int applicationID);

        // Update details of an existing application into application table of DB
        Task UpdateAsync(ApplicationDTO application);
        
        // Save change to application table of DB
        Task SaveAsync();
        
        // TRUE if applicationID exists in application table of DB, otherwise FALSE
        bool ApplicationExists(int applicationID);
    }
}
