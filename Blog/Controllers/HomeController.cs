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
using Blog.Data;

namespace Blog.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserProfileService _userProfile;
        private readonly IArticleService _articleService;
        private readonly IVoteSevice _voteSevice;
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext context, IUserProfileService userProfile, IArticleService articleService, IVoteSevice voteSevice)
        {
            db = context;
            _userProfile = userProfile;
            _articleService = articleService;
            _voteSevice = voteSevice;
        }

        public async Task<IActionResult> Index() => View(await db.Articles.ToListAsync());

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

        [HttpPost]
        public async Task<IActionResult> AddVoteForArticle(int id, VoteStatus voteStatus)
        {
            await _voteSevice.AddVoteForArticle(id, voteStatus, await _userProfile.GetUserId(User));
            return RedirectToAction("Article", new { id });
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
