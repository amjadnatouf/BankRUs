using System.ComponentModel.DataAnnotations;

namespace BankRUs.Api.Dtos.BankAccounts;

public record CreateBankAccountRequestDto(
    [Required]
    Guid UserId
);