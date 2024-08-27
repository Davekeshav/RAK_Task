using System.Net;
using DemoProject.Controllers.Api;
using DemoProject.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using RestSharp;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using DemoProject.Models.Pages;
using EPiServer.Cms.Shell.UI.Rest.Models;
using EPiServer.Core;
using EPiServer;

namespace TestDemoProject {
    public class ArticleApiControllerTests
    {
        private readonly Mock<IContentRepository> _contentRepositoryMock;
        private readonly ArticleApiController _controller;
    
        public ArticleApiControllerTests()
        {
            _contentRepositoryMock = new Mock<IContentRepository>();
            var httpClient = new HttpClient();
            _controller = new ArticleApiController(httpClient, _contentRepositoryMock.Object);
        }
    
        [Fact]
        public void Create_ShouldReturnSuccessMessage()
        {
            // Arrange
            var article = new JsonRequestPropertyModel
            {
                ParentLink = new ParentLink { Id = 5 },
                Name = "Test Article",
                Title = new Title { Value = "Test Title" },
                Content = new Content { Value = "Test Content" }
            };
    
            var articlePage = new ArticlePage
            {
                ContentLink = new ContentReference(1),
                Name = "Test Article",
                Title = "Test Title",
                Content = new XhtmlString("Test Content")
            };
    
            _contentRepositoryMock
                .Setup(repo => repo.Get<ArticlePage>(It.IsAny<ContentReference>()))
                .Returns(articlePage);
    
            _contentRepositoryMock
                .Setup(repo => repo.GetDefault<ArticlePage>(It.IsAny<ContentReference>()))
                .Returns(articlePage);
    
            // Act
            var result = _controller.Create(article) as JsonResult;
    
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Article has created successfully", result.Value);
        }
    
        [Fact]
        public async Task Get_ShouldReturnArticle()
        {
            // Arrange
            var article = new JsonRequestPropertyModel
            {
                Name = "Test Article",
                Title = new Title { Value = "Test Title" },
                Content = new Content { Value = "Test Content" }
            };
    
            var httpClient = new HttpClient();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(article);
            var httpContent = new StringContent(json);
            var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = httpContent };
    
            // Mocking HttpClient behavior
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
    
            // Act
            var result = await _controller.Get(1) as JsonResult;
    
            // Assert
            Assert.NotNull(result);
            Assert.Equal(article, result.Value);
        }
    
        [Fact]
        public void Update_ShouldReturnSuccessMessage()
        {
            // Arrange
            var updatedArticle = new JsonRequestPropertyModel
            {
                Title = new Title { Value = "Updated Title" }
            };
    
            var existingArticle = new ArticlePage
            {
                Name = "Existing Article",
                ContentLink = new ContentReference(1),
                Title = "Old Title"
            };
    
            _contentRepositoryMock
                .Setup(repo => repo.Get<ArticlePage>(It.IsAny<ContentReference>()))
                .Returns(existingArticle);
    
            // Act
            var result = _controller.Update(1, updatedArticle) as JsonResult;
    
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Existing Article has updated successfully", result.Value);
        }
    
        [Fact]
        public void Delete_ShouldReturnSuccessMessage()
        {
            // Arrange
            var article = new ArticlePage
            {
                Name = "Article to Delete",
                ContentLink = new ContentReference(1)
            };
    
            _contentRepositoryMock
                .Setup(repo => repo.Get<ArticlePage>(It.IsAny<ContentReference>()))
                .Returns(article);
    
            // Act
            var result = _controller.Delete(1) as JsonResult;
    
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Article to Delete has Deleted successfully", result.Value);
        }
    
        [Fact]
        public void Create_ShouldReturnBadRequestOnException()
        {
            // Arrange
            _contentRepositoryMock
                .Setup(repo => repo.Get<ArticlePage>(It.IsAny<ContentReference>()))
                .Throws(new Exception("Database error"));
    
            var article = new JsonRequestPropertyModel();
    
            // Act
            var result = _controller.Create(article) as BadRequestObjectResult;
    
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Database error", result.Value);
        }
    
        //[Fact]
        //public async Task Get_ShouldReturnBadRequestOnException()
        //{
        //    // Arrange
        //    var httpClient = new HttpClient();
        //    var httpClientFactory = new Mock<IHttpClientFactory>();
        //    httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        //    _controller = new ArticleApiController(httpClient, _contentRepositoryMock.Object);
        //    var ex = new HttpRequestException("Request failed");
    
        //    // Act
        //    var result = await _controller.Get(1) as BadRequestObjectResult;
    
        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("Request failed", result.Value);
        //}
    
        [Fact]
        public void Update_ShouldReturnNotFoundWhenArticleDoesNotExist()
        {
            // Arrange
            _contentRepositoryMock
                .Setup(repo => repo.Get<ArticlePage>(It.IsAny<ContentReference>()))
                .Returns((ArticlePage)null);
    
            var updatedArticle = new JsonRequestPropertyModel();
    
            // Act
            var result = _controller.Update(1, updatedArticle) as NotFoundResult;
    
            // Assert
            Assert.NotNull(result);
        }
    
        [Fact]
        public void Delete_ShouldReturnNotFoundWhenArticleDoesNotExist()
        {
            // Arrange
            _contentRepositoryMock
                .Setup(repo => repo.Get<ArticlePage>(It.IsAny<ContentReference>()))
                .Returns((ArticlePage)null);
    
            // Act
            var result = _controller.Delete(1) as NotFoundResult;
    
            // Assert
            Assert.NotNull(result);
        }
    }
}



