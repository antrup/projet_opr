using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugTrackerAPI.Controllers
{
    /// <summary>
    /// API dedicated to the tickets related processes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private ILogger<TicketsController> _logger;

        public TicketsController(ITicketRepository ticketRepository, ILogger<TicketsController> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
        }

        /// <summary>
        /// Return all tickets with paging and sorting
        /// </summary>
        [Authorize(Roles = "RegisteredUser")]
        [HttpGet]
        public async Task<ActionResult<ApiResult<TicketDTO>>> GetAllAsync(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null)
        {
            _logger.LogInformation(LogEvents.GetItems, "Getting tickets");
            try
            {
                var result = await _ticketRepository.GetAllAsync(pageIndex, pageSize, sortColumn, sortOrder).ConfigureAwait(false);
                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Getting tickets exception: {ex}", ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Return all tickets matching specified owner id with paging and sorting
        /// </summary>
        [Authorize(Roles = "RegisteredUser")]
        [HttpGet("owner/{ownerID}")]
        public async Task<ActionResult<ApiResult<TicketDTO>>> GetByOwnerAsync(
            string ownerID,
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null)
        {
            _logger.LogInformation(LogEvents.GetItems, "Getting tickets for owner {ownerID}", ownerID);
            if (ownerID == "me")
                ownerID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var result = await _ticketRepository.GetByOwnerAsync(ownerID, pageIndex, pageSize, sortColumn, sortOrder).ConfigureAwait(false);
                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Getting tickets for owner {ownerID} exception: {ex}", ownerID, ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Return all tickets matching specified creator id with paging and sorting
        /// </summary>
        [Authorize(Roles = "RegisteredUser")]
        [HttpGet("creator/{creatorID}")]
        public async Task<ActionResult<ApiResult<TicketDTO>>> GetByCreatorAsync(
           string creatorID,
           int pageIndex = 0,
           int pageSize = 10,
           string? sortColumn = null,
           string? sortOrder = null)
        {
            _logger.LogInformation(LogEvents.GetItems, "Getting tickets for creator {creatorID}", creatorID);
            if (creatorID == "me")
                creatorID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var result = await _ticketRepository.GetByCreatorAsync(creatorID, pageIndex, pageSize, sortColumn, sortOrder).ConfigureAwait(false);
                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Getting tickets for creator {creatorID} exception: {ex}", creatorID, ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Return details of a ticket
        /// </summary>
        [Authorize(Roles = "RegisteredUser")]
        [HttpGet("{ticketID}")]
        public async Task<ActionResult<TicketDTO>> GetByIdAsync(int ticketID)
        {
            _logger.LogInformation(LogEvents.GetItem, "Getting ticket {ticketID}", ticketID);
            try
            {
                var result = await _ticketRepository.GetByIdAsync(ticketID).ConfigureAwait(false);
                return result;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(LogEvents.GetItemNotFound, "Getting ticket {ticketid} NOT FOUND: {ex}", ticketID, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Getting ticket {ticketid} exception: {ex}", ticketID, ex); ;
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Allow dev users to take over a ticket (change state to in progress and owner id)
        /// </summary>
        [Authorize(Roles = "DevUser")]
        [HttpGet("take/{ticketID}")]
        public async Task<IActionResult> Take(int ticketID)
        {
            var takerID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation(LogEvents.UpdateItem, "Taking ticket {ticketID} by {takerID}", ticketID, takerID);
            try
            {
                await _ticketRepository.TakeAsync(ticketID, takerID).ConfigureAwait(false);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(LogEvents.UpdateItemNotFound, "Taking ticket {ticketid} by {takerID} NOT FOUND: {ex}", ticketID, takerID, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Taking ticket {ticketid} by {takerID} exception: {ex}", ticketID, takerID, ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Allow owner to close a ticket (change state to closed)
        /// </summary>
        [Authorize(Roles = "DevUser")]
        [HttpGet("close/{ticketID}")]
        public async Task<IActionResult> Close(int ticketID)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation(LogEvents.UpdateItem, "Closing ticket {ticketID} by {userID}", ticketID, userID);
            try
            {
                await _ticketRepository.CloseAsync(ticketID, userID);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(LogEvents.UpdateItemNotFound, "Closing ticket {ticketid} by {userID} NOT FOUND: {ex}", ticketID, userID, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Closing ticket {ticketid} by {userID} exception: {ex}", ticketID, userID, ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Insert a new ticket
        /// </summary>
        [Authorize(Roles = "RegisteredUser")]
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromForm] NewTicket newTicket)
        {
            var creatorID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation(LogEvents.InsertItem, "Inserting ticket {subject} by {creatorID}", newTicket.Subject, creatorID);
            try
            {
                await _ticketRepository.InsertAsync(newTicket, creatorID).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Inserting ticket {subject} by {creatorID} exception: {ex}", newTicket.Subject, creatorID, ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Delete a ticket (dev users only)
        /// </summary>
        [Authorize(Roles = "DevUser")]
        [HttpDelete("{ticketID}")]
        public async Task<IActionResult> DeleteAsync(int ticketID)
        {
            _logger.LogInformation(LogEvents.DeleteItem, "Deleting ticket {ticketID}", ticketID);
            try
            {
                await _ticketRepository.DeleteAsync(ticketID);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(LogEvents.UpdateItemNotFound, "Deleting ticket {ticketid} NOT FOUND: {ex}", ticketID, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Deleting ticket {ticketid} exception: {ex}", ticketID, ex);
                return Problem(ex.Message);
            }
        }
    }
}
