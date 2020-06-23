using Blog.Contracts;
using Blog.Contracts.IService;
using Blog.Data;
using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class VoteSevice : IVoteSevice
    {
        private readonly ApplicationDbContext db;

        public VoteSevice(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task AddVoteForArticle(int id, VoteStatus voteStatus, string userId)
        {
            Article article = await db.Articles.FindAsync(id);
            Vote vote = article.Votes?.FirstOrDefault(u => u.UserId == userId);

            if(vote == null)
            {
                db.Votes.Add(new Vote
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

            await db.SaveChangesAsync();
        }
    }
}
