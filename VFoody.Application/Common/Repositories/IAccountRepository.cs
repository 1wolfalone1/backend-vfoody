using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IAccountRepository : IBaseRepository<Account>
{
    int CountAll(int excludedRoleId);
    List<Account> GetAll(int excludedRoleId, int pageNum, int pageSize);
    Account GetCustomerAccount(string email, string password);
    Account? GetAccountByEmail(string email);

    bool CheckExistAccountByPhoneNumber(string phoneNumber);
    Task<Account> GetAccountByPhoneNumber(string firebaseUserPhoneNumber);
    
    Account? GetAccountWithBuildingByEmail(string email);
}
