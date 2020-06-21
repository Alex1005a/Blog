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
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Blog.Contracts.IService;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IUserProfileService _userProfile;
        private readonly IArticleService _articleService;

        public HomeController(ILogger<HomeController> logger, IRepositoryWrapper repoWrapper, IUserProfileService userProfile, IArticleService articleService)
        {
            _logger = logger;
            _repoWrapper = repoWrapper;
            _userProfile = userProfile;
            _articleService = articleService;
        }

        public async Task<IActionResult> Index() => View(await _repoWrapper.Article.FindAll().ToListAsync());

        [HttpGet]
        public IActionResult CreateArticle() => View();

        [HttpPost]
        public async Task<IActionResult> CreateArticle(CreateArticleViewModel model)
        {
            await _articleService.Create(model, await _userProfile.GetUserId(User));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Article(int id) => View(await _articleService.GetArticleById(id));

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
