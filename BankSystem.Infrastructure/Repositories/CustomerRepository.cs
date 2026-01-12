using BankSystem.Domain.Entities;
using BankSystem.Domain.Interfaces;
using BankSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Customer?> GetByIdentificationNumberAsync(string identificationNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.IdentificationNumber == identificationNumber);
    }

    public async Task<Customer?> GetWithAccountsAsync(Guid customerId)
    {
        return await _dbSet
            .Include(c => c.Accounts)
            .FirstOrDefaultAsync(c => c.Id == customerId);
    }
}