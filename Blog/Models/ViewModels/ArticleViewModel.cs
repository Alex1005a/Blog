using Blog.Domain;
using System.Collections.Generic;

namespace Blog.Models.ViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }

        public User User { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
