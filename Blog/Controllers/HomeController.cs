using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Blog.Contracts.Serviceinterfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using MediatR;
using Blog.Features.Queries.GetArticleById;
using Blog.Features.Queries.GetPageArticles;
using Blog.Features.Commands.AddComment;
using System;
using Blog.Features.Commands.AddVote;

namespace Blog.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserProfileService _userProfile;
        private readonly IArticleService _articleService;
        private readonly IMediator _mediator;

        public HomeController(IUserProfileService userProfile, IArticleService articleService, IMediator mediator)
        {
            _userProfile = userProfile;
            _articleService = articleService;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(int page = 1, string searchString = null)
        {
            return View(await _mediator.Send(new GetPageArticles(page, searchString)));
        }

        [HttpPost]
        public async Task<JsonResult> AddFile(IFormFileCollection uploads)
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
            return RedirectToArticle(id);
        }

        [HttpGet]
        public async Task<IActionResult> Article(int id)
        {
            var articleById = new GetViewArticleById(id);
            return View(await _mediator.Send(articleById));
        }

        [HttpPost]
        public async Task<IActionResult> AddVoteForArticle(int id, VoteStatus voteStatus)
        {
            var userId = _userProfile.GetUserId(User);
            var article = await _mediator.Send(new GetArticleById(id));
            await _mediator.Send(new AddVote(voteStatus, await userId, article));
            return RedirectToArticle(id);
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentForArticle(int id, string text)
        {
            var userId = _userProfile.GetUserId(User);
            var article = await _mediator.Send(new GetArticleById(id));
            await _mediator.Send(new AddComment(text, await userId, article, DateTime.Now));
            return RedirectToArticle(id);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        private IActionResult RedirectToArticle(int id)
        {
            return RedirectToAction("Article", new { id });
        }
    }
}
