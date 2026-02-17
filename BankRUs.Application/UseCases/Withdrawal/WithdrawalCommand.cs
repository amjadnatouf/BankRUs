namespace BankRUs.Application.UseCases.Withdrawal;

public record WithdrawalCommand(
    Guid BankAccountId,
    decimal Amount,
    string? Reference
);