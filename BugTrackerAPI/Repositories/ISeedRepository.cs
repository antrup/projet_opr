namespace BugTrackerAPI.Repositories
{
    public interface ISeedRepository
    {
        // Initialise default roles (RegisteredUser, DevUser, AdminUser)
        Task CreateDefaultRolesAsync();

        // Initialise default admin user (password defined in Secrets file) 
        Task CreateDefaultAdminAsync();
    }
}
