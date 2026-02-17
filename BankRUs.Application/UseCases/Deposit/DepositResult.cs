namespace BankRUs.Application.UseCases.Deposit;

public record DepositResult(
    Guid TransactionId,
    Guid UserId,
    string Type,
    decimal Amount,
    string Currency,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);