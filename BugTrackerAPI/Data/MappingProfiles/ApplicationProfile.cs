using AutoMapper;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.Models;

namespace BugTrackerAPI.Data.MappingProfiles
{
    // Mapping for automapper - application related data
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Application, ApplicationDTO>();
            CreateMap<ApplicationDTO, Application>();
            CreateMap<NewApplicationDTO, Application>();
            CreateMap<ApiResult<Application>, ApiResult<ApplicationDTO>>();
        }
    }
}

