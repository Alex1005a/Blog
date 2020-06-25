using Blog.Models;
using Dapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Blog.Entities.Models
{
    public class Article
    {
        [Key]
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }

        public string UserId { get; private set; }
        public virtual User User { get; private set; }
        public virtual ICollection<Vote> Votes { get; private set; }

        public Article(string title, string body, string userId)
        {
            Title = title;
            Body = body;
            UserId = userId;
            Votes = new List<Vote>();
        }

        public void UpdateArticle(string title, string body)
        {
            Title = title;
            Body = body;
        }
    }
}
