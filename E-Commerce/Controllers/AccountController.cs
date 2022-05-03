using E_Commerce.Models;
using E_Commerce.repositry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountrepostry _accountRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _usermanager;
        public AccountController(IAccountrepostry accountRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> usermanager)
        {
            _accountRepository = accountRepository;
            _signInManager = signInManager;
            _usermanager = usermanager;

        }

        [Route("singup")]
        public IActionResult singup()
        {
            return View();
        }
        [Route("singup")]
        [HttpPost]
        public async Task<IActionResult> singup(singupmodel singup)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.createuserAsync(singup);
                if (!result.Succeeded)
                {


                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);

                    }
                    ViewBag.status = false;
                    ViewBag.alertmesaage = "Account Creation Fail";
                    return View(singup);

                }
                else
                {
                    ViewBag.status = true;
                    ViewBag.alertmesaage = "Account Created Successfully";
                }

                ModelState.Clear();
                return RedirectToAction("ConfirmEmails", new { email = singup.email });
            }
            else
            {
                ViewBag.status = false;
                ViewBag.alertmesaage = "Account Creation Fail";
            }
            ViewBag.alertmesaage = "Account Creation Fail";

            return View(singup);
        }
        [Route("Login")]
        public IActionResult login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (User.IsInRole("Admin"))
                {
                    return Redirect("~/Admin/Admin/Index/");
                }
                else if (User.IsInRole("Customer"))
                {
                    return Redirect("~/Home/Index/");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }


        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> login(singinmodel signInModel, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignInAsync(signInModel);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    ViewBag.status = true;
                    ViewBag.alertmesaage = "Account Login Successfully";

                    var user = await _usermanager.FindByEmailAsync(signInModel.Email);
                    // Get the roles for the user
                    var roles = await _usermanager.GetRolesAsync(user);
                    foreach (string item in roles)
                    {
                        if (item == "Admin")
                        {

                            return Redirect("~/Admin/Admin/Index/");

                        }
                        else if (item == "Customer")
                        {
                            return Redirect("~/Home/Index/");

                        }
                    }
                }

                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Account is Block Try after 20Min");
                    ViewBag.status = false;
                    ViewBag.alertmesaage = "Your Account Is Lock Try Again 20Min";
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credetnial");
                    ViewBag.status = false;
                    ViewBag.alertmesaage = "Login Fail";
                }


            }

            ViewBag.status = false;
            ViewBag.alertmesaage = "Login Fail";

            return View(signInModel);
        }
        public async Task<ActionResult> logout()
        {
            await _accountRepository.logoutsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmails(string uid, string token, string email)
        {

            EmailCofirmModel model = new EmailCofirmModel
            {
                Email = email
            };

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _accountRepository.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    model.EmailVerified = true;
                }
            }

            return View(model);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmails(EmailCofirmModel model)
        {
            var user = await _accountRepository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    model.EmailVerified = true;
                    return RedirectToAction("login", "Account");
                }

                await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                model.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong.");
            }
            return View(model);
        }
        [AllowAnonymous, HttpGet("fotgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous, HttpPost("fotgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswodModel forgot)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.GetUserByEmailAsync(forgot.Email);
                if (user != null)
                {
                    await _accountRepository.GenerateForgotPasswordTokenAsync(user);
                }
                ModelState.Clear();
                forgot.EmailSent = true;
            }
            return View(forgot);
        }
        [AllowAnonymous, HttpGet("Reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel reset = new ResetPasswordModel
            {
                Token = token,
                UserId = uid
            };
            return View();
        }

        [AllowAnonymous, HttpPost("Reset-password")]
        public async Task<IActionResult> Resetpassword(ResetPasswordModel reset)
        {
            if (ModelState.IsValid)
            {
                reset.Token = reset.Token.Replace(' ', '+');
                var res = await _accountRepository.ResetPasswordAsync(reset);
                if (res.Succeeded)
                {
                    ModelState.Clear();
                    reset.IsSuccess = true;
                    return View(reset);
                }
                foreach (var errorMessage in res.Errors)
                {
                    ModelState.AddModelError("", errorMessage.Description);
                }
            }
            return View(reset);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

