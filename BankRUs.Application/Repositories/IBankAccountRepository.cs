using BankRUs.Domain.Entities;

namespace BankRUs.Application.Repositories;

public interface IBankAccountRepository
{
    Task<BankAccount> CreateAsync(BankAccount bankAccount);
    Task<BankAccount?> GetByIdAsync(Guid id);
    Task<BankAccount?> GetByAccountNumberAsync(string accountNumber);
    Task<IEnumerable<BankAccount>> GetByUserIdAsync(string userId);
    Task UpdateAsync(BankAccount bankAccount);
}