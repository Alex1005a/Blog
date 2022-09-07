using Blog.Contracts.Serviceinterfaces;
using Blog.Data;
using Blog.Entities.ViewModels;
using Blog.Models;
using Blog.ViewModels;
using Dropbox.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;
        private readonly ILogger<UserProfileService> _logger;
        private readonly DropboxClient client;
        private readonly ApplicationDbContext db;

        public UserProfileService(UserManager<User> userManager, SignInManager<User> signInManager, 
            EmailService emailService, ILogger<UserProfileService> logger, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
            client =  new DropboxClient(Startup.DropboxToken);
            db = context;
        }

        public async Task<string> Login(LoginViewModel loginViewModel)
        {
            string email = _userManager.NormalizeEmail(loginViewModel.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return "Вы не подтвердили свой email";
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginViewModel.Password, loginViewModel.RememberMe, false);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.UserName} login in app");
                return null;
            }
            else
            {
                return "Неправильный логин и (или) пароль";
            }
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logout in app");
        }

        public async Task<IdentityResult> Register(RegisterViewModel registerViewModel, IUrlHelper Url, HttpContext httpContext)
        {
            User user = new User { Email = registerViewModel.Email, UserName = registerViewModel.UserName };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            _logger.LogInformation("User add in db");

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code },
                        protocol: httpContext.Request.Scheme);

                await _emailService.SendMessageAsync(registerViewModel.Email, "Confirm your account",
                    $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
                return result;
            }

            return result;
        }

        public async Task<string> GetUserId(ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);
            return await _userManager.GetUserIdAsync(user);
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal userClaims)
        {
            return await _userManager.GetUserAsync(userClaims);
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) return false;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                _logger.LogInformation("User email confirm");
                return true;
            }               
            else return false;
        }

        public AuthenticationProperties ExternalLogin(string provider, string redirectUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return properties;
        }

        public async Task GetExternalLoginInfoAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var user1 = await _userManager.FindByLoginAsync(info.LoginProvider, info.Principal.FindFirstValue(ClaimTypes.NameIdentifier));
            
            if (user1 != null)
            {
                await _signInManager.SignInAsync(user1, isPersistent: false);
                _logger.LogInformation($"User {user1.UserName} login with {info.LoginProvider}");
                return;
            }

            if (info.Principal.FindFirstValue(ClaimTypes.Email) == null)
            {
                _logger.LogDebug($"User {info.Principal.FindFirstValue(ClaimTypes.NameIdentifier)} don't have email");
            }

            var user = new User { UserName = info.Principal.FindFirstValue(ClaimTypes.Name), Email = info.Principal.FindFirstValue(ClaimTypes.Email) };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"New User login in app with {info.ProviderDisplayName}");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
            }
            else
            {
                foreach(var a in result.Errors)
                {
                    _logger.LogError(a.Code + " : " + a.Description);
                }
            }
        }
        public async Task<string> AddImgUrl(IFormFile uploadedFile, User user)
        {
            string remotePath = "/IMG/" + Guid.NewGuid().ToString() + uploadedFile.FileName;

            var uploadTask = client.Files.UploadAsync(remotePath, body: uploadedFile.OpenReadStream());

            var result = await client.Sharing.CreateSharedLinkWithSettingsAsync(remotePath);
            var url = result.Url;
            string newUrl = url.Substring(0, url.Length - 4) + "raw=1";
            user.AddImgUrl(newUrl, db);

            await uploadTask;

            return newUrl;
        }
    }
}
