using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;
using BankRUs.Intrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<(IEnumerable<Transaction> Items, int TotalCount)> GetByBankAccountIdAsync(
        Guid bankAccountId,
        int page,
        int pageSize,
        DateTime? from,
        DateTime? to,
        string? type,
        string sort)
    {
        var query = _context.Transactions
            .Where(t => t.BankAccountId == bankAccountId);

        // Filter by date range
        if (from.HasValue)
            query = query.Where(t => t.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(t => t.CreatedAt <= to.Value);

        // Filter by type
        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(t => t.Type == type.ToLower());

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Sort
        query = sort.ToLower() == "asc"
            ? query.OrderBy(t => t.CreatedAt)
            : query.OrderByDescending(t => t.CreatedAt);

        // Pagination
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}