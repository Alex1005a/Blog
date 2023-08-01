using Blog.Domain;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Contracts.Serviceinterfaces
{
    public interface IUserProfileService
    {
        Task<IdentityResult> Register(RegisterViewModel registerViewModel, IUrlHelper Url, HttpContext httpContext);
        Task<string> Login(LoginViewModel loginViewModel);
        Task Logout();
        Task<string> GetUserId(ClaimsPrincipal user);
        Task<User> GetUserAsync(ClaimsPrincipal user);
        Task<bool> ConfirmEmail(string userId, string code);
        AuthenticationProperties ExternalLogin(string provider, string redirectUrl);
        Task GetExternalLoginInfoAsync();
        Task<string> AddImgUrl(IFormFile uploadedFile, User user);
    }
}
