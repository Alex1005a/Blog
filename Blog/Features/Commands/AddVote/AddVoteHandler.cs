using Blog.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Features.Commands.AddVote
{
    public class AddVoteHandler : IRequestHandler<AddVote, Unit>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IVoteRepository _voteRepository;

        public AddVoteHandler(IArticleRepository articleRepository, IVoteRepository voteRepository)
        {
            _articleRepository = articleRepository;
            _voteRepository = voteRepository;
        }

        public async Task<Unit> Handle(AddVote request, CancellationToken cancellationToken)
        {
            var vote = await _voteRepository.GetById(request.UserId, request.ArticleId);
            if(vote != null)
                return Unit.Value;
            var newVote = new Vote
            (
                request.VoteStatus,
                request.UserId,
                request.ArticleId
            );
            await _voteRepository.Add(newVote);
            if (newVote.Status == VoteStatus.Upvote)
                await _articleRepository.IncrementRating(request.ArticleId);
            else
                await _articleRepository.DecrementRating(request.ArticleId);
            return Unit.Value;          
        }
    }
}
