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
        public int Id { get; set; }
        public VoteStatus Status { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}
