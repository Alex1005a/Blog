using Blog.Contracts;
using Blog.Contracts.IRepository;
using Blog.Data;

namespace Blog.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private IUserRepository _user;
        private IArticleRepository _article;
        private IVoteRepository _vote;

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_applicationDbContext);
                }

                return _user;
            }
        }

        public IArticleRepository Article
        {
            get
            {
                if (_article == null)
                {
                    _article = new ArticleRepository(_applicationDbContext);
                }

                return _article;
            }
        }

        public IVoteRepository Vote
        {
            get
            {
                if (_vote == null)
                {
                    _vote = new VoteRepository(_applicationDbContext);
                }

                return _vote;
            }
        }

        public RepositoryWrapper(ApplicationDbContext _applicationDbContext)
        {
            this._applicationDbContext = _applicationDbContext;
        }

        public void Save()
        {
            _applicationDbContext.SaveChanges();
        }
    }
}
