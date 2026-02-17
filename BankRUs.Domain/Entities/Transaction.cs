namespace BankRUs.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public required Guid BankAccountId { get; set; }
    public required string Type { get; set; } // "deposit" or "withdrawal"
    public required decimal Amount { get; set; }
    public required string Currency { get; set; } = "SEK";
    public string? Reference { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required decimal BalanceAfter { get; set; }

    // Navigation property
    public BankAccount? BankAccount { get; set; }
}