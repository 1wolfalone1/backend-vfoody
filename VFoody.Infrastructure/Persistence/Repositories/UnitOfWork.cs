using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Infrastructure.Persistence.Contexts;

namespace VFoody.Infrastructure.Persistence.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private const string ErrorNotOpenTransaction = "You not open transaction yet!";
    private const string ErrorAlreadyOpenTransaction = "Transaction already open";
    private bool isTransaction;
    private VFoodyContext context;

    public UnitOfWork()
    {
        this.context = new VFoodyContext();
    }

    public bool IsTransaction
    {
        get
        {
            return this.isTransaction;
        }
    }

    internal VFoodyContext Context { get => this.context; }

    public async Task BeginTransactionAsync()
    {
        if (this.isTransaction)
        {
            throw new Exception(ErrorAlreadyOpenTransaction);
        }

        isTransaction = true;
    }

    public async Task CommitTransactionAsync()
    {
        if (!this.isTransaction)
        {
            throw new Exception(ErrorNotOpenTransaction);
        }

        await this.context.SaveChangeAsync().ConfigureAwait(false);
        this.isTransaction = false;
    }

    public void RollbackTransaction()
    {
        if (!this.isTransaction)
        {
            throw new Exception(ErrorNotOpenTransaction);
        }

        this.isTransaction = false;

        foreach (var entry in this.context.ChangeTracker.Entries())
        {
            entry.State = EntityState.Detached;
        }
    }
}
