
namespace BankSystem.Domain.Interfaces;
public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    IAccountRepository Accounts { get; }
    ITransactionRepository Transactions { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
