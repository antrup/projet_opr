using AutoMapper;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerAPI.Data.MappingProfiles
{
    // Mapping for automapper - users related data
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<BugTrackerUser, UserInfoDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom<RolesResolver>());
            CreateMap<RegisterDTO, BugTrackerUser>()
                .ForSourceMember(u => u.Password, opt => opt.DoNotValidate())
                .ForSourceMember(u => u.RoleDev, opt => opt.DoNotValidate())
                .ForSourceMember(u => u.RoleAdmin, opt => opt.DoNotValidate())
                .ForMember(dest => dest.SecurityStamp, opt => opt.NullSubstitute(Guid.NewGuid().ToString()));
        }
    }

    // Recipe to retrieve roles for a specific user (using UserManager from Identity)
    public class RolesResolver : IValueResolver<BugTrackerUser, UserInfoDTO, IEnumerable<string>>
    {
        private readonly UserManager<BugTrackerUser> _userManager;

        public RolesResolver(UserManager<BugTrackerUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<string> Resolve(BugTrackerUser source, UserInfoDTO destination, IEnumerable<string> member, ResolutionContext context)
        {
            var roles_list = _userManager.GetRolesAsync(source).Result;
            return roles_list;
        }
    }
}
