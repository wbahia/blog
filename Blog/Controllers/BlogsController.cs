using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{

    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository BlogPostRepository;
        private readonly IBlogPostLikeRepository BlogPostLikeRepository;
        private readonly UserManager<IdentityUser> UserManager;
        private readonly SignInManager<IdentityUser> SignInManager;

        public BlogsController(IBlogPostRepository blogPostRepository, 
                               IBlogPostLikeRepository blogPostLikeRepository,
                               SignInManager<IdentityUser> signInManager,
                               UserManager<IdentityUser> userManager)
        {
            BlogPostRepository = blogPostRepository;
            BlogPostLikeRepository = blogPostLikeRepository;
            SignInManager = signInManager;
            UserManager = userManager;
        }

        

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var post = await BlogPostRepository.GetAsync(urlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModel();
            var liked = false;

            if (post != null)
            {
                
                if(SignInManager.IsSignedIn(User))
                {
                    //get like for this user/post
                    var likes = await BlogPostLikeRepository.GetLikes(post.Id);
                    var userId = UserManager.GetUserId(User);

                    if(userId != null)
                    {
                        if (likes.Any(x => x.UserId == Guid.Parse(userId)))
                        {
                            liked = true;
                        }
                    }
                }
                
                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = post.Id,
                    Content = post.Content,
                    PageTitle = post.PageTitle,
                    Author = post.Author,
                    FeaturedImageUrl = post.FeaturedImageUrl,
                    Heading = post.Heading,
                    PublishedDate = post.PublishedDate,
                    ShortDescription = post.ShortDescription,
                    Slug = post.Slug,
                    Visible = post.Visible,
                    Tags = post.Tags,
                    TotalLikes = await BlogPostLikeRepository.GetTotalLikes(post.Id),
                    Liked = liked,
                };

                
            }
            return View(blogDetailsViewModel);
        }

        
    }
}