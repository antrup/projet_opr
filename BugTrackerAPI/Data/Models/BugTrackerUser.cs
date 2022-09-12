using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace BugTrackerAPI.Data.Models
{
    public class BugTrackerUser : IdentityUser
    {
        [ForeignKey("CreatorId")]
        public ICollection<Ticket>? CreatedTickets { get; set; } // Every ticket has a creator

        [ForeignKey("OwnerId")]
        public ICollection<Ticket>? OwnedTickets { get; set; } // Tickets may have an owner (dev in charge of the tiket resolution)

        public ICollection<BugTrackerUserRole> UserRoles { get; set; } = null!; // Allow to easily query roles for a specific user
    }
}
