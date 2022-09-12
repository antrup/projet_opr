using BugTrackerAPI.Data.DTO;

namespace BugTrackerAPI.Repositories
{
    public interface IStatsRepository
    {
        // return stats regarding tickets
        Task<StatsDTO> Getasync(string userID);
    }
}
