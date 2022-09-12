using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerAPI.Data.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        /// <summary>
        /// Ticket unique identifier
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Ticket subject
        /// </summary>
        [Required]
        public string Subject { get; set; } = null!;

        /// <summary>
        /// Application at issue
        /// </summary>
        [Required]
        public int Application { get; set; }

        /// <summary>
        /// Ticket state (new, in progress, closed)
        /// </summary>
        public string State { get; set; } = null!;

        /// <summary>
        /// Ticket creator
        /// </summary>
        [Required]
        public string CreatorId { get; set; } = null!;

        /// <summary>
        /// Dev user in charge of ticket resolution
        /// </summary>
        public string? OwnerId { get; set; }

        /// <summary>
        /// Ticket creation date
        /// </summary>
        [Required]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Issue description
        /// </summary>
        [Required]
        public string Description { get; set; } = null!;

        /// <summary>
        /// Screenshot (optionnal, stored as an array of byte in the DB)
        /// </summary>
        public byte[]? Screenshot { get; set; } = null!;


    }
}
