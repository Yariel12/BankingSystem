using BankSystem.Domain.Interfaces;
using BankSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankSystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BankDbContext _context;
    private IDbContextTransaction? _transaction;

    public ICustomerRepository Customers { get; }
    public IAccountRepository Accounts { get; }
    public ITransactionRepository Transactions { get; }

    public UnitOfWork(BankDbContext context)
    {
        _context = context;
        Customers = new CustomerRepository(_context);
        Accounts = new AccountRepository(_context);
        Transactions = new TransactionRepository(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}