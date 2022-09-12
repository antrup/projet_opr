using AutoMapper;
using BugTrackerAPI.Data.DTO;
using BugTrackerAPI.Data.Models;

namespace BugTrackerAPI.Data.MappingProfiles
{
    // Mapping for automapper - tickets related data
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketDTO>();
            CreateMap<NewTicket, Ticket>();
            CreateMap<ApiResult<Ticket>, ApiResult<TicketDTO>>();
            CreateMap<IFormFile, byte[]>()
                .ConvertUsing(new FormFileConverter()); // Custom conversion recipe, see  FormFileConverter.cs
        }
    }
}
