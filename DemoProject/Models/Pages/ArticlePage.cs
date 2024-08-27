using EPiServer.DataAnnotations;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace DemoProject.Models.Pages
{
    [ContentType(DisplayName = "ArticlePage", GUID = "60A743FB-4E24-44D3-8B86-2E8BEDDDC99D", Description = "Simple article page to render article data")]
    public class ArticlePage : PageData
    {
        [CultureSpecific]
        [Display(
            Name = "Title",
            Description = "Title of the article",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Content",
            Description = "Content of the article",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual XhtmlString Content { get; set; }
    }
}
