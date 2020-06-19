using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Models;
using Blog.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IUserProfileService _userProfile;
        public HomeController(ILogger<HomeController> logger, IRepositoryWrapper repoWrapper, IUserProfileService userProfile)
        {
            _logger = logger;
            _repoWrapper = repoWrapper;
            _userProfile = userProfile;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Count = await _userProfile.GetUserId(User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
