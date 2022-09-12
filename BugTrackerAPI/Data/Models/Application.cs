using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerAPI.Data.Models
{
    [Table("Applications")]
    public class Application
    {
        /// <summary>
        /// Application unique identifier
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Application name
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        [ForeignKey("Application")]
        public ICollection<Ticket>? Tickets { get; set; } = null!; // Every ticket is linked to an application
    }
}
