using Blog.Data;
using Blog.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Blog.Models
{
    public class User : IdentityUser
    {

        public virtual ICollection<Article> Articles { get; private set; }
        public virtual ICollection<Vote> Votes { get; private set; }

        public User()
        {
            Articles = new List<Article>();
            Votes = new List<Vote>();
        }

        public void AddArticle(Article article, ApplicationDbContext db)
        {
            Articles.Add(article);
            db.SaveChanges();
        }
    }
}
