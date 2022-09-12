namespace BugTrackerAPI.Data.DTO
{
    public class LoginResultDTO
    {

        public bool Success { get; set; }

        public string Message { get; set; } = null!;

        /// <summary>
        /// JWT token if the login attempt is successful, NULL if not
        /// </summary>
        public string? Token { get; set; }

        public string? Id { get; set; }

        public string? UserName { get; set; }

        public IEnumerable<string>? Roles { get; set; }
    }
}
