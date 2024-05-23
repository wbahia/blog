using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {

        public ITagRepository TagRepository { get; }

        public AdminTagsController(ITagRepository tagRepository) 
        {
            this.TagRepository = tagRepository;
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

            await this.TagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags =  await this.TagRepository.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await this.TagRepository.GetAsync(id);
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
            
            var updatedTag = await this.TagRepository.UpdateAsync(tag);

            if(updatedTag != null){
                //show ok

            }
            else{
                //show error
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });

        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedTag = await this.TagRepository.DeleteAsync(id);

            if(deletedTag != null){
                //show ok
                return RedirectToAction("List");

            }
            else{
                //show error
            }

            
            return RedirectToAction("List");

        }

    }
}