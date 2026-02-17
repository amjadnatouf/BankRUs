namespace BankRUs.Application.UseCases.GetTransactions;

public record GetTransactionsResult(
    Guid AccountId,
    string Currency,
    decimal Balance,
    PagingInfo Paging,
    IEnumerable<TransactionInfo> Items
);

public record PagingInfo(
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);

public record TransactionInfo(
    Guid TransactionId,
    string Type,
    decimal Amount,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);