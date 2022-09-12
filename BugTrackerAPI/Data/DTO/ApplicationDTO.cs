using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Data.DTO
{
    public class ApplicationDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression("^[a-zA-Z0-9àâäéèêëîïôöùûüÿç \\-_'\"]+$",
            ErrorMessage = "Valid Characters include (A-Z) (a-z) (0-9) (àâäéèêëîïôöùûüÿç) ('\") (-_) (space)")]
        [MaxLength(100, ErrorMessage = "Name: 15 characters max")]
        public string Name { get; set; } = null!;
    }
}
