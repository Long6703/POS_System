using POS.Shared.DTOs;
using POS.Shared.Entities;
using static POS_API.Repository.IRepository.IGenericRepository;

namespace POS_API.Repository.IRepository
{
    public interface IShopRepository : IGenericRepository<Shop>
    {
        Task<IEnumerable<Shop>> GetShopsWithDetailsAsync();
        Task<Shop> GetShopByIdWithDetailsAsync(int id);
        Task<IEnumerable<ShopDto>> GetShopsWithShopOwnerAsync();
        Task<bool> UpdateDelete(int shopId);

    }
}
