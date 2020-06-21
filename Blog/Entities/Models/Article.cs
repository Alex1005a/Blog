using Blog.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }

        public Article()
        {
            Votes = new List<Vote>();
        }
    }
}
