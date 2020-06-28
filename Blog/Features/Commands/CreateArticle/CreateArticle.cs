using Blog.Contracts.CommandInterfeces;
using Blog.Models;

namespace Blog.Features.Commands.CreateArticle
{
    public class CreateArticle : ICommand
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public User User { get; set; }
    }
}
