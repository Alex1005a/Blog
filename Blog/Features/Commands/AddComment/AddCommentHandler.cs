using AutoMapper;
using Blog.Data;
using Blog.Entities.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Features.Commands.AddComment
{
    public class AddCommentHandler : IRequestHandler<AddComment, Unit>
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper _mapper;

        public AddCommentHandler(ApplicationDbContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(AddComment model, CancellationToken cancellationToken)
        {
            var comment = _mapper.Map<Comment>(model);
            model.Article.AddComment(comment);
            await db.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
