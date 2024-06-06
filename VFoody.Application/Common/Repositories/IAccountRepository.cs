using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IAccountRepository : IBaseRepository<Account>
{
    Account GetCustomerAccount(string email, string password);
    Account? GetAccountByEmail(string email);

    bool CheckExistAccountByPhoneNumber(string phoneNumber);
    Task<Account> GetAccountByPhoneNumber(string firebaseUserPhoneNumber);
}
