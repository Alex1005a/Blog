using Blog.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Blog.Models
{
    public class User : IdentityUser
    {

        public virtual ICollection<Article> Articles { get; set; }

        public User()
        {
            Articles = new List<Article>();
        }
    }
}
