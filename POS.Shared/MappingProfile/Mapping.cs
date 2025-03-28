using AutoMapper;
using POS.Shared.DTOs;
using POS.Shared.Entities;

namespace POS.Shared.MappingProfile
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
