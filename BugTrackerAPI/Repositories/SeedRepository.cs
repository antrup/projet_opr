using BugTrackerAPI.Data;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerAPI.Repositories
{
    public class SeedRepository : ISeedRepository
    {
        private readonly BugTrackerEntities _context;
        private readonly RoleManager<BugTrackerRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        // Services injection
        public SeedRepository(
            BugTrackerEntities context,
            RoleManager<BugTrackerRole> roleManager,
            IConfiguration configuration,
            IUserRepository userRepository)
        {
            _context = context;
            _roleManager = roleManager;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task CreateDefaultRolesAsync()
        {
            // create the default roles (if they don't exist yet)
            if (await _roleManager.FindByNameAsync(roleName.RegisteredUser).ConfigureAwait(false) == null)
                await _roleManager.CreateAsync(new BugTrackerRole() { Name = "RegisteredUser" }).ConfigureAwait(false);

            if (await _roleManager.FindByNameAsync(roleName.DevUser).ConfigureAwait(false) == null)
                await _roleManager.CreateAsync(new BugTrackerRole() { Name = "DevUser" }).ConfigureAwait(false);

            if (await _roleManager.FindByNameAsync(roleName.Administrator).ConfigureAwait(false) == null)
                await _roleManager.CreateAsync(new BugTrackerRole() { Name = "Administrator" }).ConfigureAwait(false);

            await SaveAsync();
        }

        public async Task CreateDefaultAdminAsync()
        {
            await _userRepository.RegisterAsync(new RegisterDTO()
            {
                UserName = defaultAdmin.Username,
                Email = defaultAdmin.Email,
                Password = _configuration["DefaultPasswords:Administrator"], // password is defined in Secrets file
                RoleDev = true,
                RoleAdmin = true
            }).ConfigureAwait(false);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        public static class roleName
        {
            // default role names
            public const string RegisteredUser = "RegisteredUser";
            public const string DevUser = "DevUser";
            public const string Administrator = "Administrator";
        }

        public static class defaultAdmin
        {
            // default admin user (password defined in Secrets file)
            public const string Email = "admin@email.com";
            public const string Username = "BT_Admin";
        }
    }
}
