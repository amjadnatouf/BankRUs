using System.ComponentModel.DataAnnotations;

namespace BankRUs.Api.Dtos.Transactions.Deposit;

public record DepositRequestDto(
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount can have maximum 2 decimal places")]
    decimal Amount,

    [MaxLength(140, ErrorMessage = "Reference cannot exceed 140 characters")]
    string? Reference
);