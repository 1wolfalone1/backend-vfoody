using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class VerificationCodeRepository : BaseRepository<VerificationCode>, IVerificationCodeRepository
{
    public VerificationCodeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}