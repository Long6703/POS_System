using AutoMapper;
using POS.Shared.DTOs;
using POS.Shared.DTOs.Order;
using POS.Shared.Entities;
using static POS_API.Enum.Enums;

namespace POS.Shared.MappingProfile
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<UserShop, UserShopDTO>();

            CreateMap<Shop, ShopDto>()
            .ForMember(dest => dest.EmailShopOwner,
                opt => opt.MapFrom(src =>
                    src.UserShops
                        .Where(us => us.Role == UserRole.ShopOwner)
                        .Select(us => us.User.Email)
                        .FirstOrDefault()
                ));

            CreateMap<CreateShopDto, Shop>();
            CreateMap<UpdateShopDto, Shop>();

            // User mappings
            CreateMap<User, UserDTO>();

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // UserShop mappings
            CreateMap<UserShop, UserSearchDto>();

            CreateMap<UserShopRoleDto, UserShop>();

            // Order mappings
            CreateMap<CreateOrderDTO, Order>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)OrderStatus.Pending))
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) // Will be calculated in service
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore()); // Will be handled in service

            CreateMap<UpdateOrderDTO, Order>()
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) // Will be calculated in service
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore()); // Will be handled in service

            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => Enum.GetName(typeof(OrderStatus), src.Status)))
                .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop.Name));

            // OrderDetail mappings
            CreateMap<OrderDetailDTO, OrderDetail>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Price, opt => opt.Ignore()); // Will be set from Product price

            CreateMap<OrderDetail, OrderDetailResponseDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Price * src.Quantity));

            // Payment mappings
            CreateMap<CreatePaymentDTO, Payment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1)) // Paid status
                .ForMember(dest => dest.PaidAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Payment, PaymentResponseDTO>()
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => Enum.GetName(typeof(PaymentMethod), src.PaymentMethod)))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status == OrderStatus.Pending));
        }
    }
}
