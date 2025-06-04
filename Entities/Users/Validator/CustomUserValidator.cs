// Ignore Spelling: Validator

using Microsoft.AspNetCore.Identity;

namespace Entities.Users.Validator
{
    public class CustomUserValidator<TUser> : UserValidator<TUser>
     where TUser : IdentityUser
    {
        public override async Task<IdentityResult> ValidateAsync(
            UserManager<TUser> manager,
            TUser user
        )
        {
            // Call the base validator for all other checks (e.g., username, password, etc.)
            var baseResult = await base.ValidateAsync(manager, user);

            // If the base validation fails for non-email reasons, return the result
            if (!baseResult.Succeeded)
            {
                return baseResult;
            }

            // Skip email validation by removing email-related errors
            var errors = baseResult.Errors
                .Where(e => !e.Code.Contains("Email")) // Filter out email-related errors
                .ToList();

            // Return success if no errors remain, otherwise return the filtered errors
            return errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;
        }
    }
}
