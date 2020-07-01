using Blog.Contracts.CommandInterfeces;
using Blog.Data;
using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Commands.AddVote
{
    public class AddVoteHandler : ICommandHandler<AddVote>
    {
        private readonly ApplicationDbContext db;
        public AddVoteHandler(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<ICommonResult> Execute(AddVote model)
        {
            var vote = new Vote
                       (
                           model.VoteStatus,
                           model.UserId,
                           model.Article.Id
                       );
            await Task.Run(() => model.Article.AddVote(vote, db));

            return new CommonResult(vote.Id, "Vote adds", true);
        }
    }
}
