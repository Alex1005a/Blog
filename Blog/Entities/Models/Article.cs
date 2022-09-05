using Blog.Contracts.CommandInterfeces;
using Blog.Data;
using Blog.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entities.Models
{
    public class Article
    {
        private readonly HashSet<Vote> _votes;
        private readonly HashSet<Comment> _comments;

        [Key]
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string UserId { get; private set; }
        public virtual User User { get; private set; }
        public virtual IReadOnlyCollection<Vote> Votes => _votes?.ToList();
        public virtual IReadOnlyCollection<Comment> Comments => _comments?.ToList();
        protected Article() { }

        public Article(string title, string body, string userId)
        {
            Title = title;
            Body = body;
            UserId = userId;
            _votes = new HashSet<Vote>();
            _comments = new HashSet<Comment>();
        }

        public void UpdateArticle(string title, string body)
        {
            Title = title;
            Body = body;
        }

        public async Task<ICommonResult> AddVote(Vote vote, ApplicationDbContext db)
        {
            var Vote = Votes.FirstOrDefault(u => u.UserId == vote.UserId);
            if (Vote == null)
            {
                Vote = vote;
                _votes.Add(vote);
            }
            else Vote.UpdateStatus(vote.Status);

            await db.SaveChangesAsync();

            return new CommonResult(Vote.Id, "Vote add", true);
        }
        public async Task<ICommonResult> AddComment(Comment comment, ApplicationDbContext db)
        {
            _comments.Add(comment);

            await db.SaveChangesAsync();

            return new CommonResult(comment.Id, "comment add", true);
        }
    }
}
