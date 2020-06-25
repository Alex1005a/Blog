using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Contracts;
using Microsoft.AspNetCore.Authorization;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Blog.Contracts.IService;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using System.Collections.Generic;

namespace Blog.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserProfileService _userProfile;
        private readonly IArticleService _articleService;
        private readonly IVoteSevice _voteSevice;

        public HomeController(IUserProfileService userProfile, IArticleService articleService, IVoteSevice voteSevice)
        {
            _userProfile = userProfile;
            _articleService = articleService;
            _voteSevice = voteSevice;
        }

        public async Task<IActionResult> Index() => View(await _articleService.GetArticles());

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
            var votes = await _voteSevice.AddVoteForArticle(id, voteStatus, await _userProfile.GetUserId(User));
            //return RedirectToAction("Article", new { id });
            return PartialView("GetAssessment", votes);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
