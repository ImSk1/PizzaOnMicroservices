using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Identity.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(IIdentityServerInteractionService interaction, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _interaction = interaction;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public  IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel inputModel)
        {
            var context = await _interaction.GetAuthorizationContextAsync(inputModel.ReturnUrl);
            if (context == null)
            {
                return Redirect("~/");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(inputModel.Email);
                var result = await _signInManager.PasswordSignInAsync(user.UserName, inputModel.Password,
                    inputModel.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return Redirect(inputModel.ReturnUrl);

                }
            }
            return View(inputModel);

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();
            }
            return View("LoggedOut");
        }

    }
}
