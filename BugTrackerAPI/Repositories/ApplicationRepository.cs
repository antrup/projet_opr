using AutoMapper;
using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerAPI.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly BugTrackerEntities _context;
        private readonly IMapper _mapper;

        // DB context and AutoMapper injections
        public ApplicationRepository(BugTrackerEntities context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResult<ApplicationDTO>> GetAllAsync(
           int pageIndex = 0,
           int pageSize = 10,
           string? sortColumn = null,
           string? sortOrder = null)
        {
            if (_context.Applications == null) // DB context id not implemented
            {
                throw new NotImplementedException();
            }
            var result = await ApiResultGetter<Application>.CreateAsync(
            _context.Applications.AsNoTracking(),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder
            ).ConfigureAwait(false);

            return _mapper.Map<ApiResult<ApplicationDTO>>(result);
        }

        public async Task<ApplicationDTO> GetByIdAsync(int applicationID)
        {
            if (_context.Applications == null) // DB context id not implemented
            {
                throw new NotImplementedException();
            }
            var application = await _context.Applications.FindAsync(applicationID).ConfigureAwait(false);

            if (application == null) // ApplicationID does not exist in DB
            {
                throw new KeyNotFoundException();
            }
            return _mapper.Map<ApplicationDTO>(application);
        }

        public async Task InsertAsync(NewApplicationDTO application)
        {
            if (_context.Applications == null)
            {
                throw new NotImplementedException(); // DB context id not implemented
            }

            Application newApplication = _mapper.Map<Application>(application);

            _context.Applications.Add(newApplication);
            await SaveAsync().ConfigureAwait(false);
        }
        public async Task DeleteAsync(int applicationID)
        {
            if (_context.Applications == null)
            {
                throw new NotImplementedException(); // DB context id not implemented
            }
            var application = await _context.Applications.FindAsync(applicationID).ConfigureAwait(false);
            if (application == null) // ApplicationID does not exist in DB
            {
                throw new KeyNotFoundException();
            }

            _context.Applications.Remove(application);
            await SaveAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(ApplicationDTO application)
        {
            if (_context.Applications == null) // DB context id not implemented
            {
                throw new NotImplementedException();
            }

            var applicationToUpdate = await _context.Applications.FirstOrDefaultAsync(app => app.Id == application.Id);
            if (applicationToUpdate == null) // ApplicationID does not exist in DB
            {
                throw new KeyNotFoundException();
            }

            applicationToUpdate.Name = application.Name;
            _context.Entry(applicationToUpdate).State = EntityState.Modified;

            await SaveAsync().ConfigureAwait(false);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public bool ApplicationExists(int applicationID)
        {
            return (_context.Applications?.Any(e => e.Id == applicationID)).GetValueOrDefault();
        }
    }
}
