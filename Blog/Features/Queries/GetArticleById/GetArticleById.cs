using Blog.Contracts.Queryinterfaces;
using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetArticleById : IQuery<Article>
    {
        public int Id { get; set; }
        public GetArticleById(int id)
        {
            Id = id;
        }
    }
}
