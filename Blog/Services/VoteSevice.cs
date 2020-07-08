using Blog.Contracts.CommandInterfeces;
using Blog.Contracts.Queryinterfaces;
using Blog.Contracts.Serviceinterfaces;
using Blog.Data;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Blog.Features.Commands.AddVote;
using Blog.Features.Queries.GetArticleById;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class VoteSevice : IVoteSevice
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public VoteSevice(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<IEnumerable<Vote>> AddVoteForArticle(int articleId, VoteStatus voteStatus, string userId)
        {
            Article article = await _queryDispatcher.Execute<GetArticleById, Article>(new GetArticleById(articleId));

            await _commandDispatcher.Execute(new AddVote(voteStatus, userId, article));

            return article.Votes;
        }
    }
}
