namespace Blog.Domain
{
    public enum VoteStatus
    {
        Upvote,
        Downvote
    }

    public class Vote
    {
        public VoteStatus Status { get; private set; }

        public string UserId { get; private set; }
        public int ArticleId { get; private set; }

        protected Vote() { }

        public Vote(VoteStatus status, string userId, int articleId)
        {
            Status = status;
            UserId = userId;
            ArticleId = articleId;
        }

        public void ChangeStatus(VoteStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
