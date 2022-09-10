using Blog.Entities.Models;
using MediatR;

namespace Blog.Features.Commands.AddVote
{
    public class AddVote : IRequest
    {
        public VoteStatus VoteStatus { get; set; }
        public string UserId { get; set; }
        public Article Article { get; set; }

        public AddVote(VoteStatus VoteStatus, string UserId, Article Article)
        {
            this.VoteStatus = VoteStatus;
            this.UserId = UserId;
            this.Article = Article;
        }
    }
}
