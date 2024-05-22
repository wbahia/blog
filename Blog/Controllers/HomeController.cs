using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Repositories;

namespace Blog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IBlogPostRepository BlogPostRepository;

    public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository)
    {
        _logger = logger;
        BlogPostRepository = blogPostRepository;
    }

    public async Task<IActionResult> Index()
    {
        var blogPosts = await BlogPostRepository.GetAllAsync();
        return View(blogPosts);
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
