using Blog.Entities.Models;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Blog.Contracts.Serviceinterfaces
{
    public interface IVoteSevice
    {
        Task AddVoteForArticle(int id, VoteStatus voteStatus, string userId);
    }
}