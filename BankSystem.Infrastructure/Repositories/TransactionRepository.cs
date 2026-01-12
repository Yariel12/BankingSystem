using BankSystem.Domain.Entities;
using BankSystem.Domain.Interfaces;
using BankSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Infrastructure.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        return await _dbSet
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(Guid accountId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(t => t.AccountId == accountId
                     && t.TransactionDate >= startDate
                     && t.TransactionDate <= endDate)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }
}