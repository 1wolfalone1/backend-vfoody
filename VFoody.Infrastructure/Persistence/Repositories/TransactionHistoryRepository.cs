using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class TransactionHistoryRepository : BaseRepository<TransactionHistory>, ITransactionHistoryRepository
{
    public TransactionHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}