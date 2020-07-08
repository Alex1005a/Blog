using Blog.Contracts.Serviceinterfaces;
using Blog.Controllers;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests
{
    public class HomeControllerTests
    {     
        [Fact]
        public async Task IndexReturnsAViewResultWithAListOfUsers()
        {
            // Arrange
            var mock = new Mock<IArticleService>();
            mock.Setup(repo => repo.GetArticles(1)).Returns(Task.FromResult(new IndexViewModel
            {
                Articles = GetTestArticles(),
                PageViewModel = new PageViewModel(0, 1, 3)
            }));
            var controller = new HomeController(new Mock<IUserProfileService>().Object, mock.Object, new Mock<IVoteSevice>().Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.Model);
            Assert.Equal(GetTestArticles().Count, model.Articles.Count());
        }

        private List<Article> GetTestArticles()
        {
            var Articles = new List<Article>
            {
            };
            return Articles;
        }
        
    }
}
