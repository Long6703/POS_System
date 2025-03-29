using POS.Shared.Entities;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;
using static POS_API.Repository.IRepository.IGenericRepository;

namespace POS_API.Repository.Imp
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(POSSystemDBContext context) : base(context)
        {
        }
    }
}
