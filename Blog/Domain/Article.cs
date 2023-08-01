namespace Blog.Domain
{
    public class Article
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public int Rating { get; private set; }
        public string UserId { get; private set; }

        public Article(string title, string body, string userId)
        {
            Title = title;
            Body = body;
            UserId = userId;
        }
    }
}
