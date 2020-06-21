using Blog.Contracts;
using Blog.Controllers;
using Blog.Entities.Models;
using Blog.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests
{
    public class HomeControllerTests
    {
        /*
        [Fact]
        public void IndexReturnsAViewResultWithAListOfUsers()
        {
            // Arrange
            var mock = new Mock<IRepositoryWrapper>();
            mock.Setup(repo => repo.Article.FindAll().ToList()).Returns(GetTestArticles());
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
            Assert.Equal(GetTestArticles().Count, model.Count());
        }
        */
        private List<Article> GetTestArticles()
        {
            var Articles = new List<Article>
            {
                new Article { },
                new Article { },
                new Article { },
                new Article { }
            };
            return Articles;
        }
    }
}
