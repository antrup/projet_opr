namespace BugTrackerAPI.Data.DTO
{
    public class UserInfoDTO
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = null!;
    }
}
