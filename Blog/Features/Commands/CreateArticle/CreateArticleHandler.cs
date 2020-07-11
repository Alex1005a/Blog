﻿using AutoMapper;
using Blog.Contracts.CommandInterfeces;
using Blog.Data;
using Blog.Entities.Models;
using Ganss.XSS;
using Markdig;
using System.Threading.Tasks;

namespace Blog.Features.Commands.CreateArticle
{
    public class CreateArticleHandler : ICommandHandler<CreateArticle>
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper _mapper;
        public CreateArticleHandler(ApplicationDbContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        public async Task<ICommonResult> Execute(CreateArticle model)
        {          
            var article = _mapper.Map<Article>(model);    

            await Task.Run(() =>
                model.User.AddArticle(article, db)
            );


            return new CommonResult(article.Id, "article Add in DB!!!", true); 
        }
    }
}
