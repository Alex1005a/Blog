using Blog.Contracts;
using Blog.Contracts.IService;
using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class VoteSevice : IVoteSevice
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public VoteSevice(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task AddVoteForArticle(int id, VoteStatus voteStatus, string userId)
        {
            Article article = _repoWrapper.Article.FindByCondition(u => u.Id == id).First();
            Vote vote = article.Votes?.FirstOrDefault(u => u.UserId == userId);

            if(vote == null)
            {
                _repoWrapper.Vote.Create(new Vote
                {
                    Status = voteStatus,
                    ArticleId = id,
                    UserId = userId
                });
            }
            else
            {
                if(vote.Status != voteStatus)
                {
                    vote.Status = voteStatus;
                }
            }

            await Task.Run(() => _repoWrapper.Save());
        }
    }
}
