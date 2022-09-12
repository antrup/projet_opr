using AutoMapper;
using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerAPI.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly BugTrackerEntities _context;
        private readonly IMapper _mapper;

        // DB context and AutoMapper injection
        public TicketRepository(BugTrackerEntities context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResult<TicketDTO>> GetAllAsync(
           int pageIndex = 0,
           int pageSize = 10,
           string? sortColumn = null,
           string? sortOrder = null)
        {
            if (_context.Tickets == null) // DB context not implemented
            {
                throw new NotImplementedException();
            }
            var result = await ApiResultGetter<Ticket>.CreateAsync(
            _context.Tickets.AsNoTracking(),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder
            ).ConfigureAwait(false);

            return _mapper.Map<ApiResult<TicketDTO>>(result);
        }

        public async Task<ApiResult<TicketDTO>> GetByOwnerAsync(
         string ownerId,
         int pageIndex = 0,
         int pageSize = 10,
         string? sortColumn = null,
         string? sortOrder = null)
        {
            if (_context.Tickets == null) // DB context not implemented
            {
                throw new NotImplementedException();
            }
            var result = await ApiResultGetter<Ticket>.CreateAsync(
            _context.Tickets.AsNoTracking().Where(t => t.OwnerId == ownerId),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder
            ).ConfigureAwait(false);

            return _mapper.Map<ApiResult<TicketDTO>>(result);
        }

        public async Task<ApiResult<TicketDTO>> GetByCreatorAsync(
         string creatorId,
         int pageIndex = 0,
         int pageSize = 10,
         string? sortColumn = null,
         string? sortOrder = null)
        {
            if (_context.Tickets == null) // DB context not implemented
            {
                throw new NotImplementedException();
            }
            var result = await ApiResultGetter<Ticket>.CreateAsync(
            _context.Tickets.AsNoTracking().Where(t => t.CreatorId == creatorId),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder
            ).ConfigureAwait(false);

            return _mapper.Map<ApiResult<TicketDTO>>(result);
        }

        public async Task<TicketDTO> GetByIdAsync(int ticketID)
        {
            if (_context.Tickets == null) // DB context not implemented
            {
                throw new NotImplementedException();
            }
            var ticket = await _context.Tickets.FindAsync(ticketID).ConfigureAwait(false);

            if (ticket == null) // ticketID does not exist in DB
            {
                throw new KeyNotFoundException();
            }
            return _mapper.Map<TicketDTO>(ticket);
        }

        public async Task InsertAsync(NewTicket newTicket, string creatorID)
        {
            if (_context.Tickets == null) // DB context not implemented
            {
                throw new NotImplementedException();
            }

            Ticket ticket = _mapper.Map<Ticket>(newTicket);

            ticket.CreationDate = DateTime.Now;
            ticket.State = "new";
            ticket.CreatorId = creatorID;

            _context.Tickets.Add(ticket);
            await SaveAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int ticketID)
        {
            if (_context.Tickets == null) // DB context not implemented
            {
                throw new NotImplementedException();
            }
            var ticket = await _context.Tickets.FindAsync(ticketID).ConfigureAwait(false);

            if (ticket == null) // ticketID does not exist in DB
            {
                throw new KeyNotFoundException();
            }

            _context.Tickets.Remove(ticket);
            await SaveAsync().ConfigureAwait(false);
        }

        public async Task TakeAsync(int ticketID, string takerID)
        {
            if (_context.Tickets == null) // DB context not implemented
            {
                throw new NotImplementedException();
            }
            var ticket = await _context.Tickets.FindAsync(ticketID).ConfigureAwait(false);

            if (ticket == null) // ticketID does not exist in DB
            {
                throw new KeyNotFoundException();
            }

            if (ticket.State != "new") // Only ticket with "new" state can be taken (in progress or closed = already taken) 
            {
                throw new InvalidOperationException("Ticket already taken");
            }

            // Update ticket data
            ticket.OwnerId = takerID;
            ticket.State = "In progress";

            _context.Entry(ticket).State = EntityState.Modified;
            await SaveAsync().ConfigureAwait(false);
        }

        public async Task CloseAsync(int ticketID, string userID)
        {
            if (_context.Tickets == null) // DB context not implemented
            {
                throw new NotImplementedException();
            }
            var ticket = await _context.Tickets.FindAsync(ticketID).ConfigureAwait(false);

            if (ticket == null) // ticketID does not exist in DB
            {
                throw new KeyNotFoundException();
            }

            if (ticket.State != "In progress") // Only ticket with "in progress" state can be closed
            {
                throw new InvalidOperationException("Ticket cannot be closed");
            }

            if (ticket.OwnerId != userID) // Only ticket owner can close the ticket
            {
                throw new InvalidOperationException("Only owner can close a ticket");
            }

            // Update ticket data
            ticket.State = "Closed";

            _context.Entry(ticket).State = EntityState.Modified;
            await SaveAsync().ConfigureAwait(false);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
