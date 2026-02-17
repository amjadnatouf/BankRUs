namespace BankRUs.Application.Repositories;

public interface IUserRepository
{
    Task<bool> UserExistsAsync(Guid userId);
}