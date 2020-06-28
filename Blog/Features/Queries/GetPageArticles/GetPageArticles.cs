using Blog.Contracts;
using Blog.Contracts.Queryinterfaces;
using Blog.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Queries.GetPageArticles
{
    public class GetPageArticles : IQuery<IndexViewModel>
    {
        public int Page { get; set; }
        public GetPageArticles(int page)
        {
            Page = page;
        }
    }
}
