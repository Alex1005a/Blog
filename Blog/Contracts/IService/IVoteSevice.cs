using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Contracts.IService
{
    public interface IVoteSevice
    {
        Task AddVoteForArticle(int id, VoteStatus voteStatus, string userId);
    }
}
