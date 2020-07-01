using Blog.Data;
using Blog.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Models
{
    public class User : IdentityUser
    {
        private HashSet<Article> _articles;
        private HashSet<Vote> _votes;

        public virtual IEnumerable<Article> Articles => _articles?.ToList();
        public virtual IEnumerable<Vote> Votes => _votes?.ToList();

        public User()
        {
            _articles = new HashSet<Article>();
            _votes = new HashSet<Vote>();
        }

        public void AddArticle(Article article, ApplicationDbContext db)
        {
            _articles.Add(article);
            db.SaveChanges();
        }
    }
}
