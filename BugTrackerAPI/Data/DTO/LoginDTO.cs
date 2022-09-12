using System.ComponentModel.DataAnnotations;
namespace BugTrackerAPI.Data.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression("^[A-Za-z0-9\\-_]+$", ErrorMessage = "Valid Charactors include (A-Z) (a-z) (0-9) (- _)")]
        [MaxLength(10, ErrorMessage = "Username: 10 characters max")]
        [MinLength(4, ErrorMessage = "Username: 4 characters min")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@$!%*#?&])[a-zA-Z0-9@$!%*#?&]+$",
            ErrorMessage = "Password requires at leat one lowercase, one uppercase, one digit and one special character")]
        [MaxLength(15, ErrorMessage = "Password: 15 characters max")]
        [MinLength(7, ErrorMessage = "Password: 7 characters min")]
        public string Password { get; set; } = null!;
    }
}
