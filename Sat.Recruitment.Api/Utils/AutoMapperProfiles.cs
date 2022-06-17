using AutoMapper;
using Sat.Recruitment.Api.DTOs;
using Sat.Recruitment.Api.Entities;

namespace Sat.Recruitment.Api.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserCreationDTO, User>()
                .ReverseMap();
            CreateMap<User, UserDTO>()
                .ReverseMap();
        }
    }
}
