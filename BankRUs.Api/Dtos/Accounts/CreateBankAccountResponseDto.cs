namespace BankRUs.Api.Dtos.BankAccounts;

public record CreateBankAccountResponseDto(
    Guid BankAccountId,
    string AccountNumber
);