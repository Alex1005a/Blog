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
        */
    }
}
