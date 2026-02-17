namespace BankRUs.Application.UseCases.GetTransactions;

public record GetTransactionsQuery(
    Guid BankAccountId,
    int Page,
    int PageSize,
    DateTime? From,
    DateTime? To,
    string? Type,
    string Sort
);