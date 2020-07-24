using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Blog.Contracts.Serviceinterfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Newtonsoft.Json;

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

        //[Route("Index/page{page:int}")]
        public async Task<IActionResult> Index(int page = 1, string searchString = null) => View(await _articleService.GetArticles(page, searchString));


        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFileCollection uploads)
        {
            string url = await _userProfile.AddImgUrl(uploads.First(), await _userProfile.GetUserAsync(User));

            return Json(new { url, success = true });
        }

        [HttpGet]
        public IActionResult CreateArticle() => View();

        [HttpPost]
        public async Task<IActionResult> CreateArticle(CreateArticleViewModel model)
        {
            int id = await _articleService.Create(model, await _userProfile.GetUserAsync(User));
            return RedirectToAction("Article", new { id } );
        }

        [HttpGet]
        public async Task<IActionResult> Article(int id) => View(await _articleService.GetArticleById(id));

        [HttpPost]
        public async Task<IActionResult> AddVoteForArticle(int id, VoteStatus voteStatus)
        {
            await _voteSevice.AddVoteForArticle(id, voteStatus, await _userProfile.GetUserId(User));
            return RedirectToAction("Article", new { id });
            //return PartialView("GetAssessment", votes);
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentForArticle(int id, string text)
        {
            await _articleService.AddComment(id, text, await _userProfile.GetUserId(User));
            return RedirectToAction("Article", new { id });
            //return PartialView("GetAssessment", votes);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
