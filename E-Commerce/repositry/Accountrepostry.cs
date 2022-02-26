using E_Commerce.Models;
using E_Commerce.services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.repositry
{
    public class Accountrepostry : IAccountrepostry
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Isendmail _emailService;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _rolemanager;
        
        
        public Accountrepostry(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, Isendmail emailService,
            IConfiguration configuration, RoleManager<IdentityRole> rolemanager)
        {
            _usermanager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
            _rolemanager = rolemanager;
        }
        public ICollection<ApplicationUser> alluser()
        {
            return _usermanager.Users.Where(x=>x.UserName!="yamikgandhi@gmail.com").ToList();
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _usermanager.FindByEmailAsync(email);
        }
        
        public async Task<IdentityResult> createuserAsync(singupmodel singup)
        {
            var User = new ApplicationUser()
            {
                PhoneNumber = singup.Phone,
                fname = singup.Name,
                Email = singup.email,
                UserName = singup.email
                
            };
            var result = await _usermanager.CreateAsync(User, singup.Password);
            
            if (result.Succeeded)
            {
                await createrole();
                await _usermanager.AddToRoleAsync(User, "Customer");
                
                await GenerateEmailConfirmationTokenAsync(User);
                
            }
            return result;
        }
        private async Task createrole()
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name ="Customer",
                NormalizedName="Customer"
            };
            IdentityResult result = await _rolemanager.CreateAsync(identityRole);
               
        }
        
        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var token = await _usermanager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationEmail(user, token);
            }
        }
        public async Task GenerateForgotPasswordTokenAsync(ApplicationUser user)
        {
            var token = await _usermanager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendForgotPasswordEmail(user, token);
            }
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            
            return await _usermanager.ConfirmEmailAsync(await _usermanager.FindByIdAsync(uid), token);
            
        }
        public async Task<ApplicationUser> getuser(singinmodel singin)
        {
            return await _usermanager.FindByIdAsync(singin.Email);
        }
        public async Task<SignInResult> PasswordSignInAsync(singinmodel signInModel)
        {
           return await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password,signInModel.RememberMe, true);
          
        }
    

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            return await _usermanager.ResetPasswordAsync(await _usermanager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }

        public async Task logoutsync()
        {
            await _signInManager.SignOutAsync();
        }

        
        private async Task SendEmailConfirmationEmail(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

            UserEmail options = new UserEmail
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.fname),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await _emailService.SendEmailConfirmationEmail(options);
        }
        /*public bool deleteuser(string id)
        {

            var res = _usermanager.Users.FirstOrDefault(x =>x.Id==id);
            if ( res != null)
            {
                _usermanager.DeleteAsync(res);
            }

             return(_usermanager.Users.FirstOrDefault(x => x.Id == id)==null);
            
        }*/
       
        private async Task SendForgotPasswordEmail(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:ForgotPassword").Value;

            UserEmail options = new UserEmail
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.fname),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };
            await _emailService.SendEmailForForgotPassword(options);
        }
    }
}
