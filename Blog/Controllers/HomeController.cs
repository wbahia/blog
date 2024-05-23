using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Repositories;
using Blog.Models.ViewModels;

namespace Blog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogPostRepository BlogPostRepository;
    private readonly ITagRepository TagRepository;

    public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
    {
        _logger = logger;
        BlogPostRepository = blogPostRepository;
        TagRepository = tagRepository;
    }

    public async Task<IActionResult> Index()
    {
        var blogPosts = await BlogPostRepository.GetAllAsync();
        var tags = await TagRepository.GetAllAsync();
        var model = new HomeViewModel{
            BlogPosts = blogPosts,
            Tags = tags
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
