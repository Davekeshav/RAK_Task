using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using DemoProject.Models;
using EPiServer.Shell.UI.Messaging.Internal;
using System.Net;
using DemoProject.Models.Pages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DemoProject.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleApiController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        private readonly IContentRepository _contentRepository;
        public ArticleApiController(HttpClient httpClient, IContentRepository contentRepository)
        {
            _httpClient = httpClient;
            _contentRepository = contentRepository;
        }

        // Create an article using API post call
        [HttpPost]
        [Route("createArticle")]
        public IActionResult Create([FromBody] JsonRequestPropertyModel article)
        {
            try { 
                  var parentLink = ContentReference.StartPage;
                  var articlea = _contentRepository.Get<ArticlePage>(new ContentReference(article.ParentLink.Id));
                  var articleContent = _contentRepository.GetDefault<ArticlePage>(articlea.ContentLink);
                  articleContent.Name = article.Name;
                  articleContent.Title = article.Title.Value;
                  articleContent.Content = new XhtmlString(article.Content.Value);
                  
                  _contentRepository.Save(articleContent, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
                  
                  return new JsonResult(articleContent.Name + " has created successfully");
            }
            catch (Exception ex)
            {
                  return BadRequest(ex.Message);
            }
        }

        // Read an article using API get call
        [HttpGet]
        [Route("getArticle")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                //var token = GetToken();
                using var client = new HttpClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"https://localhost:5000/api/episerver/v3.0/content/{id}");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonRequestPropertyModel>(responseBody);
                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update an article using API put call
        [HttpPut]
        [Route("updateArticle")]
        public IActionResult Update(int id, [FromBody] JsonRequestPropertyModel updatedArticle)
        {
            try
            {
                var article = _contentRepository.Get<ArticlePage>(new ContentReference(id));
                if (article == null)
                {
                    return NotFound();
                }
                var articleData = article.CreateWritableClone() as ArticlePage;
                articleData.Title = updatedArticle.Title.Value; ;
                articleData.Content = new XhtmlString (updatedArticle.Content.Value);
                _contentRepository.Save(articleData, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);

                return new JsonResult(article.Name + " has updated successfully");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // Delete an article using delete API call
        [HttpDelete]
        [Route("deleteArticle")]
        public IActionResult Delete(int id)
        {
            try
            {
                var article = _contentRepository.Get<ArticlePage>(new ContentReference(id));
                if (article == null)
                {
                    return NotFound();
                }

                _contentRepository.Delete(article.ContentLink, true);

                return new JsonResult(article.Name + " has Deleted successfully");
            }
            catch (Exception ex) {

                return BadRequest();
            }
        }

    }
}
