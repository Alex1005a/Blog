using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blog.Models.ViewModels;
using Blog.Contracts.Serviceinterfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using MediatR;
using Blog.Features.Queries.GetArticleById;
using Blog.Features.Queries.GetPageArticles;
using Blog.Features.Commands.AddComment;
using System;
using Blog.Features.Commands.AddVote;
using Blog.Domain;
using Blog.Features.Commands.CreateArticle;
using AutoMapper;
using Blog.Models;

namespace Blog.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserProfileService _userProfile;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HomeController(
            IUserProfileService userProfile, 
            IMediator mediator,
            IMapper mapper)
        {
            _userProfile = userProfile;
            _mediator = mediator;
            _mapper = mapper;
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
            var request = _mapper.Map<CreateArticle>(model);
            request.UserId = await _userProfile.GetUserId(User);
            int id = await _mediator.Send(request);
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
            var userId = await _userProfile.GetUserId(User);
            await _mediator.Send(new AddVote(voteStatus, userId, id));
            return RedirectToArticle(id);
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentForArticle(int id, string text)
        {
            var userId = await _userProfile.GetUserId(User);
            await _mediator.Send(new AddComment(text, userId, DateTime.Now));
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
