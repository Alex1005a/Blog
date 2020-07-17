using Blog.Contracts.CommandInterfeces;
using Blog.Data;
using Blog.Entities.Events;
using Blog.Extensions;
using Blog.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entities.Models
{
    public class Article
    {
        private readonly HashSet<Vote> _votes;

        [Key]
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string UserId { get; private set; }
        public virtual User User { get; private set; }
        public virtual IReadOnlyCollection<Vote> Votes => _votes?.ToList();
        protected Article() { }

        public Article(string title, string body, string userId)
        {
            Title = title;
            Body = body;
            UserId = userId;
            _votes = new HashSet<Vote>();
        }

        public void UpdateArticle(string title, string body)
        {
            Title = title;
            Body = body;
        }

        public async Task<CommonResult> AddVote(Vote vote, ApplicationDbContext db, IModel client)
        {
            var Vote = Votes.FirstOrDefault(u => u.UserId == vote.UserId);
            if (Vote == null)
            {
                Vote = vote;
                _votes.Add(vote);
            }
            else Vote.UpdateStatus(vote.Status);

            await Task.Run(() => client.Send(new AddVoteEvent { VoteId = Vote.Id, Time = DateTime.Now }));

            await db.SaveChangesAsync();

            return new CommonResult(Vote.Id, "Vote add", true);
        }
    }
}
