using BankRUs.Application.Repositories;
using BankRUs.Intrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BankRUs.Intrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> UserExistsAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user is not null;
    }
}