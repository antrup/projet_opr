using Microsoft.AspNetCore.Identity;


namespace BugTrackerAPI.Data.Models
{
    public class BugTrackerRole : IdentityRole
    {
        public ICollection<BugTrackerUserRole> UserRoles { get; set; } = null!; // Allow to easily query roles for a specific user
    }
}
