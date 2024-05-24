using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class VerificationCodeRepository : BaseRepository<VerificationCode>, IVerificationCodeRepository
{
    public VerificationCodeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public IEnumerable<VerificationCode> FindByAccountIdAndCodeTypeAndStatus(int accountId, int codeType, int status)
    {
        return DbSet.Where(v => v.AccountId == accountId && v.CodeType == codeType && v.Status == status).ToList();
    }

    public VerificationCode? FindByCodeAndStatusAndEmail(string code, int status, string email)
    {
        return DbSet.FirstOrDefault(v => v.Code == code && v.Status == status && v.Account.Email == email);
    }
}