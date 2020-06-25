using Blog.Contracts;

namespace Blog.Features.Commands.CreateArticle
{
    public class CreateArticle : ICommand
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
    }
}
