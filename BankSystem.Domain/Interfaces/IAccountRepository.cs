using BankSystem.Domain.Entities;

namespace BankSystem.Domain.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account?> GetByAccountNumberAsync(string accountNumber);
        Task<IEnumerable<Account>> GetAccountsByCustomerIdAsync(Guid customerId);
        Task<Account?> GetWithTransactionsAsync(Guid accountId);
    }
}
