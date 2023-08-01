using AutoMapper;
using Blog.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Features.Commands.AddComment
{
    public class AddCommentHandler : IRequestHandler<AddComment, Unit>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public AddCommentHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(AddComment request, CancellationToken cancellationToken)
        {
            var comment = _mapper.Map<Comment>(request);
            await _commentRepository.Add(comment);
            return Unit.Value;
        }
    }
}
