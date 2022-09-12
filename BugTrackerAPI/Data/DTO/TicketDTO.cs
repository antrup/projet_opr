namespace BugTrackerAPI.Data.DTO
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Subject { get; set; } = null!;
        public int Application { get; set; }
        public string State { get; set; } = null!;
        public string CreatorId { get; set; } = null!;
        public string? OwnerId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; } = null!;
        public byte[]? Screenshot { get; set; } = null!;
    }
}
