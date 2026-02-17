using BankRUs.Domain.Entities;

namespace BankRUs.Application.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<(IEnumerable<Transaction> Items, int TotalCount)> GetByBankAccountIdAsync(
        Guid bankAccountId,
        int page,
        int pageSize,
        DateTime? from,
        DateTime? to,
        string? type,
        string sort);
}