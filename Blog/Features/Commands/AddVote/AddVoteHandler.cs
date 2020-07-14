using Blog.Contracts.CommandInterfeces;
using Blog.Data;
using Blog.Entities.Events;
using Blog.Entities.Models;
using Blog.Extensions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Commands.AddVote
{
    public class AddVoteHandler : ICommandHandler<AddVote>
    {
        private readonly ApplicationDbContext db;
        private readonly IModel client;

        public AddVoteHandler(ApplicationDbContext context, IModel _client)
        {
            db = context;
            client = _client;
        }

        public async Task<ICommonResult> Execute(AddVote model)
        {
            var vote = new Vote
                       (
                           model.VoteStatus,
                           model.UserId,
                           model.Article.Id
                       );

            return await Task.Run(() => model.Article.AddVote(vote, db, client));          
        }
    }
}
