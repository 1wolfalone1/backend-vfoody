using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    public AccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public Account GetCustomerAccount(string email, string password)
    {
        return this.DbSet.SingleOrDefault(a => a.Email == email 
                                               && a.Password == password);
    }

    public Account? GetAccountByEmail(string email)
    {
        return DbSet.SingleOrDefault(a => a.Email == email);
    }

    public bool CheckExistAccountByPhoneNumber(string phoneNumber)
    {
        return DbSet.Any(a => a.PhoneNumber == phoneNumber);
    }
}

