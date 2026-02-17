namespace BankRUs.Api.Dtos.Transactions;

public record GetTransactionsResponseDto(
    Guid AccountId,
    string Currency,
    decimal Balance,
    PagingDto Paging,
    IEnumerable<TransactionItemDto> Items
);

public record PagingDto(
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);

public record TransactionItemDto(
    Guid TransactionId,
    string Type,
    decimal Amount,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);