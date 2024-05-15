using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
