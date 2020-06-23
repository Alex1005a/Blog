using Blog.Contracts;
using Blog.Contracts.IService;
using Blog.Data;
using Blog.Entities.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class VoteSevice : IVoteSevice
    {
        private readonly ApplicationDbContext db;
        private readonly ILogger<VoteSevice> _logger;

        public VoteSevice(ApplicationDbContext context, ILogger<VoteSevice> logger)
        {
            db = context;
            _logger = logger;
        }

        public async Task AddVoteForArticle(int articleId, VoteStatus voteStatus, string userId)
        {
            Article article = await db.Articles.FindAsync(articleId);
            Vote vote = article.Votes?.FirstOrDefault(u => u.UserId == userId);

            if(vote == null)
            {
                db.Votes.Add(new Vote
                (
                    voteStatus,
                    userId,
                    articleId
                ));
                _logger.LogInformation($"User with Id {userId} create vote to article with Id {articleId}");
            }
            else
            {
                if(vote.Status != voteStatus)
                {
                    vote.UpdateStatus(voteStatus);
                    _logger.LogInformation($"User with Id {userId} update vote with Id{vote.Id}");
                }
            }

            await db.SaveChangesAsync();
        }
    }
}
