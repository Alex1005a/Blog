using Blog.Data;
using Blog.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class User : IdentityUser
    {
        private readonly HashSet<Article> _articles;
        private readonly HashSet<Vote> _votes;

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; private set; }

        public virtual IReadOnlyCollection<Article> Articles => _articles?.ToList();
        public virtual IReadOnlyCollection<Vote> Votes => _votes?.ToList();

        public User()
        {
            _articles = new HashSet<Article>();
            _votes = new HashSet<Vote>();
        }

        public async Task AddArticle(Article article, ApplicationDbContext db)
        {
            _articles.Add(article);
            await db.SaveChangesAsync();
        }

        public void AddImgUrl(string imageUrl, ApplicationDbContext db)
        {
            ImageUrl = imageUrl;
            db.SaveChanges();
        }
    }
}
