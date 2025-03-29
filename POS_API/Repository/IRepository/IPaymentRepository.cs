using POS.Shared.Entities;
using static POS_API.Repository.IRepository.IGenericRepository;

namespace POS_API.Repository.IRepository
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
    }
}
