namespace VFoody.Application.Common.Repositories;

public interface IUnitOfWork
{
    bool IsTransaction { get; }

    Task BeginTransactionAsync();
    Task SaveChangesAsync();

    Task CommitTransactionAsync();

    void RollbackTransaction();
}
