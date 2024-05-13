using Microsoft.AspNetCore.Mvc;


namespace Blog.Controllers
{
    public class AdminTagsController : Controller
    {
        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        
    }
}