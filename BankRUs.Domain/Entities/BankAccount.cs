namespace BankRUs.Domain.Entities;

public class BankAccount
{
    public Guid Id { get; set; }
    public required string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public required string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}