using Blog.Data;
using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await _blogDbContext.Tags.AddAsync(tag);
            await _blogDbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await _blogDbContext.Tags.ToListAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if(tag != null){
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
                Id = editTagRequest.Id
            };
            
            var existingTag = await _blogDbContext.Tags.FindAsync(tag.Id);

            if(existingTag != null){
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                
                await _blogDbContext.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });

        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tag = await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if(tag != null){
                _blogDbContext.Tags.Remove(tag);
                await _blogDbContext.SaveChangesAsync();
                //show sucess
                return RedirectToAction("List");
            }

            //show error
            return RedirectToAction("List");

        }

    }
}