using Blog.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public void AddVote(Vote vote)
        {
            var Vote = Votes.FirstOrDefault(u => u.UserId == vote.UserId);
            if (Vote == null)
            {
                Vote = vote;
                _votes.Add(vote);
            }
            else Vote.UpdateStatus(vote.Status);
        }
        public void AddComment(Comment comment)
        {
            _comments.Add(comment);
        }
    }
}
