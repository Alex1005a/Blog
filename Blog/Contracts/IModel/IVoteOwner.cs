using Blog.Entities.Models;

namespace Blog.Contracts.IModel
{
    public interface IVoteOwner
    {
        void UpdateStatus(VoteStatus newStatus);
    }
}
