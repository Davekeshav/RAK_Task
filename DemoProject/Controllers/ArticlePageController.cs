using DemoProject.Models.Pages;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DemoProject.Controllers
{
    public class ArticlePageController : PageController<ArticlePage>

    {
        public IActionResult Index(ArticlePage currentPage)
        {
            return View(currentPage);
        }
    }
}
