using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IVerificationCodeRepository : IBaseRepository<VerificationCode>
{
    IEnumerable<VerificationCode> FindByAccountIdAndCodeTypeAndStatus(int accountId, int codeType, int status);
    VerificationCode? FindByCodeAndStatusAndEmail(string code, int status, string email);
}
