﻿using Blog.Models;
using MediatR;

namespace Blog.Features.Commands.CreateArticle
{
    public class CreateArticle : IRequest<int>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public User User { get; set; }
    }
}
