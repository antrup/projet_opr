namespace BugTrackerAPI.Data.DTO
{
    public class StatsDTO
    {
        public int TotalTickets { get; set; }
        public int NewTickets { get; set; }
        public int InProgressTickets { get; set; }
        public int ClosedTickets { get; set; }
        public int OpenTickets30D { get; set; }
        public int TotalMyTickets { get; set; }
        public int MyInProgressTickets { get; set; }
        public int MyClosedTickets { get; set; }
        public int MyOpenTickets30D { get; set; }
        public int MyNewTickets { get; internal set; }
        public int TotalOwnedTickets { get; set; }
        public int InProgressOwnedTickets { get; set; }
        public int ClosedOwnedTickets { get; set; }

    }
}
