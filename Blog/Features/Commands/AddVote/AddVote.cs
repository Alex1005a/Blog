using Blog.Domain;
using MediatR;

namespace Blog.Features.Commands.AddVote
{
    public class AddVote : IRequest
    {
        public VoteStatus VoteStatus { get; set; }
        public string UserId { get; set; }
        public int ArticleId { get; set; }

        public AddVote(VoteStatus voteStatus, string userId, int articleId)
        {
            VoteStatus = voteStatus;
            UserId = userId;
            ArticleId = articleId;
        }
    }
}
