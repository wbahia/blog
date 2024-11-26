using Blog.Models.Domain;
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
        private readonly IBlogPostCommentRepository BlogPostCommentRepository;
        private readonly SignInManager<IdentityUser> SignInManager;

        public BlogsController(IBlogPostRepository blogPostRepository, 
                               IBlogPostLikeRepository blogPostLikeRepository,
                               SignInManager<IdentityUser> signInManager,
                               UserManager<IdentityUser> userManager,
                               IBlogPostCommentRepository blogPostCommentRepository)
        {
            BlogPostRepository = blogPostRepository;
            BlogPostLikeRepository = blogPostLikeRepository;
            SignInManager = signInManager;
            UserManager = userManager;
            BlogPostCommentRepository = blogPostCommentRepository;
        }

        

        [HttpGet]
        public async Task<IActionResult> Index(string slug)
        {
            var post = await BlogPostRepository.GetAsync(slug);
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
                
                var blogComments = await BlogPostCommentRepository.GetAllByBlogIdAsync(post.Id);
                var blogCommentsViewModel = new List<BlogCommentViewModel>();
                foreach (var blogComment in blogComments)
                {
                    var commentViewModel = new BlogCommentViewModel
                    {
                        DateAdded = blogComment.DateAdded,
                        Description = blogComment.Description,
                        UserName = (await UserManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                    };
                    blogCommentsViewModel.Add(commentViewModel);
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
                    Comments = blogCommentsViewModel
                };

                
            }
            return View(blogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel model)
        {
            if(SignInManager.IsSignedIn(User))
            {
                var comment = new BlogPostComment
                {
                    BlogPostId = model.Id,
                    DateAdded = DateTime.Now,
                    Description = model.CommentDescription,
                    UserId = Guid.Parse(UserManager.GetUserId(User))
                };
                await BlogPostCommentRepository.AddAsync(comment);
                return RedirectToAction("Index", "Blogs", new { slug = model.Slug});
            }
            return View();
        }

        
    }
}