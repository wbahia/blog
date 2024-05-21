using System.Net;
using Blog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        public readonly IImageRepository ImageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.ImageRepository = imageRepository;
            
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile formFile){
            //call a repository
            var imageURL = await this.ImageRepository.UploadAsync(formFile);

            if(imageURL == null){
                return Problem("Algo deu errado!", null, (int)HttpStatusCode.InternalServerError);
            }

            return new JsonResult(new { link = imageURL });
        }
    }
}