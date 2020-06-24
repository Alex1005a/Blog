﻿using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using System.Threading.Tasks;

namespace Blog.Contracts.IService
{
    public interface IArticleService
    {
        Task Create(CreateArticleViewModel model, string userId);
        Task<Article> GetArticleById(int Id);
    }
}
