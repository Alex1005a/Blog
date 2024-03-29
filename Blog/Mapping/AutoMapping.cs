﻿using AutoMapper;
using Blog.Domain;
using Blog.Models.ViewModels;
using Blog.Features.Commands.AddComment;
using Blog.Features.Commands.CreateArticle;

namespace Blog.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Article, ArticleViewModel>();
            CreateMap<CreateArticleViewModel, CreateArticle>();
            CreateMap<CreateArticle, Article>();
            CreateMap<AddComment, Comment>();
        }
    }
}
