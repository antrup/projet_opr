using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerAPI.Repositories
{
    public class StatsRepository : IStatsRepository
    {
        private readonly BugTrackerEntities _context;

        // DB context injection
        public StatsRepository(
            BugTrackerEntities context)
        {
            _context = context;
        }

        public async Task<StatsDTO> Getasync(string userID)
        {
            StatsDTO stats = new();
            var source = _context.Tickets.AsNoTracking();

            if (source == null)
            {
                throw new NotImplementedException();
            }

            stats.TotalTickets = await getCountAsync(source).ConfigureAwait(false);
            stats.NewTickets = await getCountByStateAsync(source, "new").ConfigureAwait(false);
            stats.InProgressTickets = await getCountByStateAsync(source, "in progress").ConfigureAwait(false);
            stats.ClosedTickets = await getCountByStateAsync(source, "closed").ConfigureAwait(false);
            stats.OpenTickets30D = await getCountByDateAsync(source, 30).ConfigureAwait(false);
            stats.TotalMyTickets = await getCountByCreatorAsync(source, userID).ConfigureAwait(false);
            stats.MyInProgressTickets = await getCountByStateAsync(
                getListByCreatorAsync(source, userID), "in progress").ConfigureAwait(false);
            stats.MyClosedTickets = await getCountByStateAsync(
                getListByCreatorAsync(source, userID), "closed").ConfigureAwait(false);
            stats.MyNewTickets = await getCountByStateAsync(
                getListByCreatorAsync(source, userID), "new").ConfigureAwait(false);
            stats.MyOpenTickets30D = await getCountByDateAsync(
                getListByCreatorAsync(source, userID), 30).ConfigureAwait(false);
            stats.TotalOwnedTickets = await getCountByOwnerAsync(source, userID).ConfigureAwait(false);
            stats.InProgressOwnedTickets = await getCountByStateAsync(
                getListByOwnerAsync(source, userID), "in progress").ConfigureAwait(false);
            stats.ClosedOwnedTickets = await getCountByStateAsync(
                getListByOwnerAsync(source, userID), "closed").ConfigureAwait(false);

            return stats;
        }


        // Count tickets from IQueryable source
        public Task<int> getCountAsync(IQueryable<Ticket> source)
        {
            return source.CountAsync();
        }

        // Count tickets with specified state from IQueryable source
        public Task<int> getCountByStateAsync(IQueryable<Ticket> source, string state)
        {
            var query = from e in source
                        where e.State == state
                        select e;
            return query.CountAsync();
        }

        // Return a query targetting tickets with specified state from IQueryable source
        public IQueryable<Ticket> getListByState(IQueryable<Ticket> source, string state)
        {
            /* functionnal form test
            from e in source
                        where e.state == state
                        select e;*/
            var query = source.Where(e => e.State == state);
            return query;
        }

        // Count tickets created within the last specified number of days from IQueryable source
        public Task<int> getCountByDateAsync(IQueryable<Ticket> source, int days)
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(days * -1);

            var query = from e in source
                        where e.CreationDate >= dateTime
                        select e;
            return query.CountAsync();
        }

        // Return a query targetting tickets created within the last specified number of days from IQueryable source
        public IQueryable<Ticket> getListbyDate(IQueryable<Ticket> source, int days)
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(days * -1);

            var query = from e in source
                        where e.CreationDate >= dateTime
                        select e;

            return query;
        }

        // Count tickets whose creatorID == userId from IQueryable source
        public Task<int> getCountByCreatorAsync(IQueryable<Ticket> source, string userId)
        {
            var query = from e in source
                        where e.CreatorId == userId
                        select e;
            return query.CountAsync();
        }

        // Return a query targetting tickets whose creatorID == userId from IQueryable source
        public IQueryable<Ticket> getListByCreatorAsync(IQueryable<Ticket> source, string userId)
        {
            var query = from e in source
                        where e.CreatorId == userId
                        select e;
            return query;
        }

        // Count tickets whose ownerID == userId from IQueryable source
        public Task<int> getCountByOwnerAsync(IQueryable<Ticket> source, string userId)
        {
            var query = from e in source
                        where e.OwnerId == userId
                        select e;
            return query.CountAsync();
        }

        // Return a query targetting tickets whose ownerID == userId from IQueryable source
        public IQueryable<Ticket> getListByOwnerAsync(IQueryable<Ticket> source, string userId)
        {
            var query = from e in source
                        where e.OwnerId == userId
                        select e;
            return query;
        }
    }
}
