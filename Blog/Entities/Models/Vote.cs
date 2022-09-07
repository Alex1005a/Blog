using Blog.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.Models
{
    public enum VoteStatus
    {
        Plus,
        Minus
    }

    public class Vote
    {
        [Key]
        public int Id { get; private set; }
        public VoteStatus Status { get; private set; }

        public string UserId { get; private set; }
        public virtual User User { get; private set; }
        public int ArticleId { get; private set; }
        public virtual Article Article { get; private set; }

        protected Vote() { }

        public Vote(VoteStatus status, string userId, int articleId)
        {
            Status = status;
            UserId = userId;
            ArticleId = articleId;
        }

        public void UpdateStatus(VoteStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
