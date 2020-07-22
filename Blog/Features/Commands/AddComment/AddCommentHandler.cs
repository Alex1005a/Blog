using AutoMapper;
using Blog.Contracts.CommandInterfeces;
using Blog.Data;
using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Commands.AddComment
{
    public class AddCommentHandler : ICommandHandler<AddComment>
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper _mapper;

        public AddCommentHandler(ApplicationDbContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        public async Task<ICommonResult> Execute(AddComment model)
        {
            var comment = _mapper.Map<Comment>(model);

            return await Task.Run(() =>
                model.Article.AddComment(comment, db)
            );
        }
    }
}
