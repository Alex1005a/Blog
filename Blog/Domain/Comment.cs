using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Domain
{
    public class Comment
    {
        [Key]
        public int Id { get; private set; }
        [MaxLength(350)]
        public string Text { get; private set; }
        public DateTime DateTime { get; private set; }

        public string UserId { get; private set; }
        public int ArticleId { get; private set; }

        protected Comment() { }

        public Comment(string text, DateTime dateTime, string userId, int articleId)
        {
            Text = text;
            DateTime = dateTime;
            UserId = userId;
            ArticleId = articleId;
        }
    }
}
