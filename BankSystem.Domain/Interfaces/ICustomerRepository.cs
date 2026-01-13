using BankSystem.Domain.Entities;

namespace BankSystem.Domain.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<Customer?> GetByIdentificationNumberAsync(string identificationNumber);
        Task<bool> EmailExistsAsync(string email);
        Task<Customer?> GetWithAccountsAsync(Guid id);
    }
}