using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce.Helper
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {

        public ApplicationClaimsPrincipalFactory(UserManager<ApplicationUser> usermanager,
            RoleManager<IdentityRole> role, IOptions<IdentityOptions> options) : base(usermanager, role, options)
        {

        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserFirstName", user.fname ?? ""));
            return identity;
        }
    }
}
