using BankSystem.Domain.Entities;
using BankSystem.Domain.Interfaces;
using BankSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Infrastructure.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(BankDbContext context) : base(context)
    {
    }

    // ✅ SOBRESCRIBE GetByIdAsync para cargar las transacciones
    public new async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(a => a.Transactions) // ← ESTO ES CRÍTICO
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _dbSet
            .Include(a => a.Transactions) // ← Agregar aquí también
            .FirstOrDefaultAsync(a => a.AccountNumber.Value == accountNumber);
    }

    public async Task<IEnumerable<Account>> GetAccountsByCustomerIdAsync(Guid customerId)
    {
        return await _dbSet
            .Include(a => a.Transactions) // ← Y aquí
            .Where(a => a.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<Account?> GetWithTransactionsAsync(Guid accountId)
    {
        return await _dbSet
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == accountId);
    }
}