using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.Controllers
{
    [Route("[controller]")]
    public class AdminTagsController : Controller
    {
        private readonly ILogger<AdminTagsController> _logger;

        public AdminTagsController(ILogger<AdminTagsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}