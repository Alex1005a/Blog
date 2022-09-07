using Blog.Contracts.CommandInterfeces;
using Blog.Entities.Models;

namespace Blog.Features.Commands.AddVote
{
    public class AddVote : ICommand
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
