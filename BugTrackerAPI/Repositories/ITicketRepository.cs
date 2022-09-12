using BugTrackerAPI.Data.DTO;

namespace BugTrackerAPI.Repositories
{
    public interface ITicketRepository
    {
        // Return all tickets with paging and sorting (as per parameters)
        Task<ApiResult<TicketDTO>> GetAllAsync(
           int pageIndex = 0,
           int pageSize = 10,
           string? sortColumn = null,
           string? sortOrder = null);

        // Return the details of ticket matching ticket ID
        Task<TicketDTO> GetByIdAsync(int ticketID);

        // Return all tickets matching specified owner id with paging and sorting
        Task<ApiResult<TicketDTO>> GetByOwnerAsync(
           string ownerID,
           int pageIndex = 0,
           int pageSize = 10,
           string? sortColumn = null,
           string? sortOrder = null);

        // Return all tickets matching specified creator id with paging and sorting
        Task<ApiResult<TicketDTO>> GetByCreatorAsync(
           string creatorID,
           int pageIndex = 0,
           int pageSize = 10,
           string? sortColumn = null,
           string? sortOrder = null);

        // Insert ticket passed in parameter into ticket table of DB
        Task InsertAsync(NewTicket newTicket, string creatorID);

        // Delete ticket whose id is passed in parameter from ticket table of DB
        Task DeleteAsync(int ticketID);

        // change ticket state to "in progress" and ticket ownerID to takerID parameter
        Task TakeAsync(int ticketID, string takerID);

        // if userID == ticket.ownerID, change ticket state to "closed"
        Task CloseAsync(int ticketID, string userID);

        // Save change to ticket table of DB
        Task SaveAsync();
    }
}
