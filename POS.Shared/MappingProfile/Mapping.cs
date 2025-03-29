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

            CreateMap<Shop, ShopDto>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count))
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.UserShops.Count));

            CreateMap<CreateShopDto, Shop>();
            CreateMap<UpdateShopDto, Shop>();

            // User mappings
            //CreateMap<User, UserDTO>()
            //    .ForMember(dest => dest.Shop, opt => opt.MapFrom(src => src.UserShops));

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // UserShop mappings
            //CreateMap<UserShop, UserSearchDto>()
            //    .ForMember(dest => dest.Shop, opt => opt.MapFrom(src => src.Shop.Name));

            CreateMap<UserShopRoleDto, UserShop>();
        }
    }
}
