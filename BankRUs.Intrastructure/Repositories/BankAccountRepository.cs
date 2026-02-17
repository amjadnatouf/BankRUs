using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;
using BankRUs.Intrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly ApplicationDbContext _context;

    public BankAccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BankAccount> CreateAsync(BankAccount bankAccount)
    {
        _context.BankAccounts.Add(bankAccount);
        await _context.SaveChangesAsync();
        return bankAccount;
    }

    public async Task<BankAccount?> GetByIdAsync(Guid id)
    {
        return await _context.BankAccounts.FindAsync(id);
    }

    public async Task<BankAccount?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _context.BankAccounts
            .FirstOrDefaultAsync(ba => ba.AccountNumber == accountNumber);
    }

    public async Task<IEnumerable<BankAccount>> GetByUserIdAsync(string userId)
    {
        return await _context.BankAccounts
            .Where(ba => ba.UserId == userId)
            .ToListAsync();
    }

    public async Task UpdateAsync(BankAccount bankAccount)
    {
        _context.BankAccounts.Update(bankAccount);
        await _context.SaveChangesAsync();
    }
}