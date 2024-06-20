using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    public AccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public int CountAll(int excludedRoleId)
    {
        return DbSet.Count(acc => acc.RoleId != excludedRoleId && acc.Status != (int)AccountStatus.Delete);
    }

    public List<Account> GetAll(int excludedRoleId, int pageNum, int pageSize)
    {
        return DbSet.Where(acc => acc.RoleId != excludedRoleId && acc.Status != (int)AccountStatus.Delete)
            .OrderByDescending(a => a.CreatedDate)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();
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

    public async Task<Account> GetAccountByPhoneNumber(string firebaseUserPhoneNumber)
    {
        return await this.DbSet.SingleOrDefaultAsync(a => a.PhoneNumber == firebaseUserPhoneNumber).ConfigureAwait(false);
    }

    public Account? GetAccountWithBuildingByEmail(string email)
    {
        return this.DbSet.Include(acc => acc.Building).SingleOrDefault(a => a.Email == email);
    }
}

