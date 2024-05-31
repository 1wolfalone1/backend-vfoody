using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IAccountRepository : IBaseRepository<Account>
{
    Account GetCustomerAccount(string email, string password);
    Account? GetAccountByEmail(string email);

    Account? GetAccountByEmailAndPhoneNumber(string email, string phoneNumber);
}
