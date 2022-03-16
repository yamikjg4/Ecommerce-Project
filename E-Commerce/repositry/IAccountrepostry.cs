using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.repositry
{
    public interface IAccountrepostry
    {
        Task<IdentityResult> createuserAsync(singupmodel singup);
        Task GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
        Task logoutsync();
        Task<SignInResult> PasswordSignInAsync(singinmodel signInModel);
        Task GenerateForgotPasswordTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);
        Task<ApplicationUser> getuser(singinmodel singinmodel);
        ICollection<ApplicationUser> alluser();
        int getcount();
        /* bool deleteuser(string id);*/
    }
}