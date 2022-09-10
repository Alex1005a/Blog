using Blog.Data;
using Blog.Entities.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Features.Commands.AddVote
{
    public class AddVoteHandler : IRequestHandler<AddVote, Unit>
    {
        private readonly ApplicationDbContext db;

        public AddVoteHandler(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<Unit> Handle(AddVote model, CancellationToken cancellationToken)
        {
            var vote = new Vote
            (
                model.VoteStatus,
                model.UserId,
                model.Article.Id
            );
            model.Article.AddVote(vote);
            await db.SaveChangesAsync();
            return Unit.Value;          
        }
    }
}
