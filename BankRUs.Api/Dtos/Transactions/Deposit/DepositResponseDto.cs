namespace BankRUs.Api.Dtos.Transactions.Deposit;

public record DepositResponseDto(
    Guid TransactionId,
    Guid UserId,
    string Type,
    decimal Amount,
    string Currency,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);