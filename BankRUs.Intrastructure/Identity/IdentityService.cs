using BankRUs.Application.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResult> CreateUserAsync(CreateUserRequest request)
    {
        // Kontrollera om personnummer redan finns
        var existingUserBySsn = await _userManager.Users
            .FirstOrDefaultAsync(u => u.SocialSecurityNumber == request.SocialSecurityNumber.Trim());

        if (existingUserBySsn is not null)
        {
            throw new InvalidOperationException("A user with this social security number already exists");
        }

        // Kontrollera om email redan finns
        var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email.Trim());

        if (existingUserByEmail is not null)
        {
            throw new InvalidOperationException("A user with this email already exists");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email.Trim(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            SocialSecurityNumber = request.SocialSecurityNumber.Trim(),
            Email = request.Email.Trim()
        };

        string password = "Secret#1";

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Unable to create user: {errors}");
        }

        await _userManager.AddToRoleAsync(user, Roles.Customer);

        return new CreateUserResult(UserId: Guid.Parse(user.Id));
    }
}
