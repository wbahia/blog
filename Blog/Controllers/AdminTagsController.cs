using Blog.Data;
using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace Blog.Controllers
{
    public class AdminTagsController : Controller
    {
        private BlogDbContext _blogDbContext;

        public AdminTagsController(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            _blogDbContext.Tags.Add(tag);
            _blogDbContext.SaveChanges();
            return View("Add");
        }


    }
}