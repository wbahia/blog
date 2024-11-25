using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository BlogPostLikeRepository;
        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            BlogPostLikeRepository = blogPostLikeRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody]AddLikeRequest request)
        {
            if (request != null)
            {
                var model = new BlogPostLike
                {
                    BlogPostId = request.BlogPostId,
                    UserId = request.UserId,
                };
                await BlogPostLikeRepository.AddLike(model);
            }
            return Ok();
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesForBlogPost([FromRoute]Guid blogPostId) 
        {
            var total = await BlogPostLikeRepository.GetTotalLikes(blogPostId);
            return Ok(total);
        }
    }
}
