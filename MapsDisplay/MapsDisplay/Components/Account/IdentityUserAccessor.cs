using MapsDisplay.Features.LocalAuthority.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace MapsDisplay.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<ApplicationUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }

        //public async Task<string> GetRequiredTokenAsync(HttpContext context)
        //{
            //var authResult = await context.AuthenticateAsync(IdentityConstants.ExternalScheme);
            //var token = authResult.Properties.GetTokenValue("access_token");
            
            //if (token is null)
            //{
            //    redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            //}

            //return token;
        //}
    }
}
