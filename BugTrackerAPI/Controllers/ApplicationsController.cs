using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerAPI.Controllers
{
    /// <summary>
    /// API dedicated to the list of applications covered by the bug tracker
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationRepository _applicationRepository;
        private ILogger<ApplicationsController> _logger;

        public ApplicationsController(IApplicationRepository applicationRepository, ILogger<ApplicationsController> logger)
        {
            _applicationRepository = applicationRepository;
            _logger = logger;
        }

        /// <summary>
        /// Return all applications with paging and sorting
        /// </summary>
        [Authorize(Roles = "RegisteredUser")]
        [HttpGet]
        public async Task<ActionResult<ApiResult<ApplicationDTO>>> GetAllAsync(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null)
        {
            _logger.LogInformation(LogEvents.GetItems, "Getting applications");
            try
            {
                var result = await _applicationRepository.GetAllAsync(pageIndex, pageSize, sortColumn, sortOrder).ConfigureAwait(false);
                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Getting applications exception: {ex}", ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Return the application matching application ID 
        /// </summary>
        [Authorize(Roles = "RegisteredUser")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDTO>> GetByIdAsync(int applicationID)
        {
            _logger.LogInformation(LogEvents.GetItem, "Getting application {applicationID}", applicationID);
            try
            {
                var result = await _applicationRepository.GetByIdAsync(applicationID).ConfigureAwait(false);
                return result;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(LogEvents.GetItemNotFound, "Getting application {applicationID} NOT FOUND: {ex}", applicationID, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Getting application {applicationID} exception: {ex}", applicationID, ex); ;
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Update the name of an application
        /// </summary>
        [Authorize(Roles = "Administrator")]
        [HttpPut("{applicationID}")]
        public async Task<IActionResult> UpdateAsync(ApplicationDTO application)
        {
            _logger.LogInformation(LogEvents.InsertItem, "Updating application {id}", application.Id);
            try
            {
                await _applicationRepository.UpdateAsync(application).ConfigureAwait(false);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(LogEvents.GetItemNotFound, "Updating application {id} NOT FOUND: {ex}", application.Id, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Updating application {id} exception: {ex}", application.Id, ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Insert a new application
        /// </summary>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> InsertAsync(NewApplicationDTO application)
        {
            _logger.LogInformation(LogEvents.InsertItem, "Inserting application {name}", application.Name);
            try
            {
                await _applicationRepository.InsertAsync(application).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Inserting application {name} exception: {ex}", application.Name, ex);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Delete an application
        /// </summary>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{applicationID}")]
        public async Task<IActionResult> DeleteAsync(int applicationID)
        {
            _logger.LogInformation(LogEvents.DeleteItem, "Deleting application {applicationID}", applicationID);
            try
            {
                await _applicationRepository.DeleteAsync(applicationID);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(LogEvents.UpdateItemNotFound, "Deleting application {applicationid} NOT FOUND: {ex}", applicationID, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvents.OtherException, "Deleting application {applicationid} exception: {ex}", applicationID, ex);
                return Problem(ex.Message);
            }
        }
    }
}
