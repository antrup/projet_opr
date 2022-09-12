using Microsoft.AspNetCore.Identity;


namespace BugTrackerAPI.Data.Models
{
    // Allow to easily query roles for a specific user
    public class BugTrackerUserRole : IdentityUserRole<string>
    {
        public virtual BugTrackerUser User { get; set; } = null!;
        public virtual BugTrackerRole Role { get; set; } = null!;
    }
}
