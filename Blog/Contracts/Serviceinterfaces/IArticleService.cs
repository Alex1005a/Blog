using Blog.Entities.ViewModels;
using Blog.Models;
using System.Threading.Tasks;

namespace Blog.Contracts.Serviceinterfaces
{
    public interface IArticleService
    {
        Task<int> Create(CreateArticleViewModel model, User user);
    }
}
