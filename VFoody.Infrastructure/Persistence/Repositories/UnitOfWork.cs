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
        get { return this.isTransaction; }
    }

    internal VFoodyContext Context
    {
        get => this.context;
    }

    public async Task BeginTransactionAsync()
    {
        if (this.isTransaction)
        {
            throw new Exception(ErrorAlreadyOpenTransaction);
        }

        isTransaction = true;
        await this.context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (!this.isTransaction)
        {
            throw new Exception(ErrorNotOpenTransaction);
        }

        await this.context.SaveChangeAsync().ConfigureAwait(false);
        await this.context.Database.CommitTransactionAsync();
        this.isTransaction = false;
    }

    public async Task SaveChangesAsync()
    {
        if (!this.isTransaction)
        {
            throw new InvalidOperationException(ErrorNotOpenTransaction);
        }

        await context.SaveChangeAsync();
    }

    public void RollbackTransaction()
    {
        if (!this.isTransaction)
        {
            throw new Exception(ErrorNotOpenTransaction);
        }

        context.Database.RollbackTransaction();
        this.isTransaction = false;

        foreach (var entry in this.context.ChangeTracker.Entries())
        {
            entry.State = EntityState.Detached;
        }
    }
}