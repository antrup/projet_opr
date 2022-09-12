using BugTrackerAPI.Data.Validation;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerAPI.Data.DTO
{
    public class NewTicket
    {
        [Required(ErrorMessage = "Subject is required.")]
        [RegularExpression("^[a-zA-Z0-9àâäéèêëîïôöùûüÿç \\-_'\"]+$",
            ErrorMessage = "Valid Characters include (A-Z) (a-z) (0-9) (àâäéèêëîïôöùûüÿç) ('\") (-_) (space)")]
        [MaxLength(100, ErrorMessage = "Subject: 100 characters max")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Application is required.")]
        [ApplicationIDValidation(ErrorMessage = "Application ID is invalid")]
        public int Application { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(3000, ErrorMessage = "Description: 3000 characters max")]
        public string Description { get; set; } = null!;

        [FileValidation(ErrorMessage = "File type not accepted")]
        [MaxFileSize(2 * 1024 * 1024, ErrorMessage = "File size is bigger than maximum")]
        public IFormFile? Screenshot { get; set; } = null!;


    }
}

