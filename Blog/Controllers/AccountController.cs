﻿using Blog.Contracts.Serviceinterfaces;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserProfileService _userProfileService;

        public AccountController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userProfileService.Register(model, Url, HttpContext);

                if (result.Succeeded) return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
           if(await _userProfileService.ConfirmEmail(userId, code)) return RedirectToHome();
           else return View("Error");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string result = await _userProfileService.Login(model);
                if (result != null)
                {
                    ModelState.AddModelError(string.Empty, result);
                    return View(model);
                } 
                else return RedirectToHome();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userProfileService.Logout();

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            return Challenge(_userProfileService.ExternalLogin(provider, redirectUrl), provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            await _userProfileService.GetExternalLoginInfoAsync();
            return RedirectToHome();
        }

        private IActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
