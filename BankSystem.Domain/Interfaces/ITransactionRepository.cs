using BankSystem.Domain.Entities;

namespace BankSystem.Domain.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(Guid accountId, DateTime startDate, DateTime endDate);
    }
}
