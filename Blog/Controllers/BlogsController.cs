using Blog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{

    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository BlogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.BlogPostRepository = blogPostRepository;
            
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var post = await this.BlogPostRepository.GetAsync(urlHandle);
            return View(post);
        }

        
    }
}