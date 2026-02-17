namespace BankRUs.Application.UseCases.Deposit;

public record DepositCommand(
    Guid BankAccountId,
    decimal Amount,
    string? Reference
);