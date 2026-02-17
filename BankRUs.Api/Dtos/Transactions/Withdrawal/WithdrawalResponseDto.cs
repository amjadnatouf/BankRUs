namespace BankRUs.Api.Dtos.Transactions.Withdrawal;

public record WithdrawalResponseDto(
    Guid TransactionId,
    Guid AccountId,
    string Type,
    decimal Amount,
    string Currency,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);