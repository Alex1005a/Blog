using AutoMapper;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;

namespace Blog.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Article, ArticleViewModel>();
        }
    }
}
