using Blog.Contracts.CommandInterfeces;
using Blog.Entities.Models;
using System;

namespace Blog.Features.Commands.AddComment
{
    public class AddComment : ICommand
    {
        public string Text { get; set; }
        public string UserId { get; set; }
        public Article Article { get; set; }
        public DateTime DateTime { get; set; }

        public AddComment(string Text, string UserId, Article Article, DateTime DateTime)
        {
            this.Text = Text;
            this.UserId = UserId;
            this.Article = Article;
            this.DateTime = DateTime;
        }
    }
}
