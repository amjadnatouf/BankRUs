using BankRUs.Application.Repositories;

namespace BankRUs.Application.UseCases.GetTransactions;

public class GetTransactionsHandler
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionsHandler(
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<GetTransactionsResult> HandleAsync(GetTransactionsQuery query)
    {
        // 1. Verifiera att bankkontot finns
        var bankAccount = await _bankAccountRepository.GetByIdAsync(query.BankAccountId);

        if (bankAccount is null)
        {
            throw new InvalidOperationException($"Bank account with ID {query.BankAccountId} not found");
        }

        // 2. Hämta transaktioner med filtrering och paginering
        var (transactions, totalCount) = await _transactionRepository.GetByBankAccountIdAsync(
            query.BankAccountId,
            query.Page,
            query.PageSize,
            query.From,
            query.To,
            query.Type,
            query.Sort
        );

        // 3. Beräkna totalt antal sidor
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        // 4. Mappa till result
        var items = transactions.Select(t => new TransactionInfo(
            TransactionId: t.Id,
            Type: t.Type,
            Amount: t.Amount,
            Reference: t.Reference,
            CreatedAt: t.CreatedAt,
            BalanceAfter: t.BalanceAfter
        ));

        return new GetTransactionsResult(
            AccountId: query.BankAccountId,
            Currency: "SEK",
            Balance: bankAccount.Balance,
            Paging: new PagingInfo(
                Page: query.Page,
                PageSize: query.PageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ),
            Items: items
        );
    }
}