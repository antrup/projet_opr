using AutoMapper;
using BugTrackerAPI.Controllers;
using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using static BugTrackerAPI.Repositories.SeedRepository;

namespace BugTrackerAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BugTrackerEntities _context;
        private readonly UserManager<BugTrackerUser> _userManager;
        private readonly RoleManager<BugTrackerRole> _roleManager;
        private readonly IMapper _mapper;

        // DB context, user and role manager (Identity), AutoMapper injection
        public UserRepository(
            BugTrackerEntities context,
            UserManager<BugTrackerUser> userManager,
            RoleManager<BugTrackerRole> roleManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public Task<ApiResult<UserInfoDTO>> GetAllAsync(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = null,
            string? sortOrder = null)
        {
            if (_userManager.Users == null) // DB context not implemented
            {
                throw new NotImplementedException();
            }
            return ApiResultGetter<UserInfoDTO>.CreateAsync(
             (from user in _userManager.Users
              select new UserInfoDTO()
              {
                  Id = user.Id,
                  UserName = user.UserName,
                  Email = user.Email,
                  Roles = (from userRole in user.UserRoles
                           join role in _roleManager.Roles on userRole.Role
                           equals role
                           select role.Name).ToList()
              }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder
            ); ;
        }

        public async Task<UserInfoDTO> GetByIdAsync(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);

            if (user == null) // provided user ID does not exist
                throw new KeyNotFoundException();

            return _mapper.Map<UserInfoDTO>(user);
        }

        public async Task RegisterAsync(RegisterDTO registerRequest)
        {
            //Check if username or email already exist in the user database
            var user = await _userManager.FindByNameAsync(registerRequest.UserName);
            if (user != null) // User with same username exists
                throw new UsernameAlreadyExistException();

            user = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (user != null) // User with same email exists
                throw new EmailAlreadyExistException();

            var new_user = _mapper.Map<BugTrackerUser>(registerRequest);

            // insert new user in the databnase
            var creation_result = await _userManager.CreateAsync(new_user, registerRequest.Password);
            if (!creation_result.Succeeded)
                throw new UserCreationFailedException(creation_result.Errors.ToString());


            // assign RegisteredUser role to the new user
            await _userManager.AddToRoleAsync(new_user, roleName.RegisteredUser);

            // assign optionnal roles to the new user
            if (registerRequest.RoleDev)
                await _userManager.AddToRoleAsync(new_user, roleName.DevUser);
            if (registerRequest.RoleAdmin)
                await _userManager.AddToRoleAsync(new_user, roleName.Administrator);

            await SaveAsync();
        }

        public async Task DeleteAsync(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null) // provided user ID does not exist
                throw new KeyNotFoundException();

            await _userManager.DeleteAsync(user);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    // Username already exists dedicated exception
    public class UsernameAlreadyExistException : Exception
    {
        public UsernameAlreadyExistException() { }

        public UsernameAlreadyExistException(string message) : base(message) { }

        public UsernameAlreadyExistException(string message, Exception innerException) : base(message, innerException) { }
    }

    // Email already exists dedicated exception
    public class EmailAlreadyExistException : Exception
    {
        public EmailAlreadyExistException() { }

        public EmailAlreadyExistException(string message) : base(message) { }

        public EmailAlreadyExistException(string message, Exception innerException) : base(message, innerException) { }
    }

    // User creation has failed for another reason
    public class UserCreationFailedException : Exception
    {
        public UserCreationFailedException() { }

        public UserCreationFailedException(string message) : base(message) { }

        public UserCreationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
