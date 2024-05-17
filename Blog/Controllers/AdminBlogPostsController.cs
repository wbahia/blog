using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Controllers
{
    public class AdminBlogPostsController : Controller
    {

        public ITagRepository TagRepository { get; }
        public IBlogPostRepository BlogPostRepository { get; }

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository) 
        {
            this.TagRepository = tagRepository;
            this.BlogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await this.TagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                Slug = addBlogPostRequest.Slug,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var existingTag = await TagRepository.GetAsync(Guid.Parse(selectedTagId));
                if (existingTag != null){
                    selectedTags.Add(existingTag);
                }
            }

            blogPost.Tags = selectedTags;
            await this.BlogPostRepository.AddAsync(blogPost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var posts =  await this.BlogPostRepository.GetAllAsync();
            return View(posts);
        }

        // [HttpGet]
        // public async Task<IActionResult> Edit(Guid id)
        // {
        //     var tag = await this.TagRepository.GetAsync(id);
        //     if(tag != null){
        //         var editTagRequest = new EditTagRequest
        //         {
        //             Id = tag.Id,
        //             Name = tag.Name,
        //             DisplayName = tag.DisplayName
        //         };
        //         return View(editTagRequest);
        //     }
        //     return View(null);
        // }

        // [HttpPost]
        // public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        // {
        //     var tag = new Tag
        //     {
        //         Name = editTagRequest.Name,
        //         DisplayName = editTagRequest.DisplayName,
        //         Id = editTagRequest.Id
        //     };
            
        //     var updatedTag = await this.TagRepository.UpdateAsync(tag);

        //     if(updatedTag != null){
        //         //show ok

        //     }
        //     else{
        //         //show error
        //     }
        //     return RedirectToAction("Edit", new { id = editTagRequest.Id });

        // }

        // [HttpGet]
        // public async Task<IActionResult> Delete(Guid id)
        // {
        //     var deletedTag = await this.TagRepository.DeleteAsync(id);

        //     if(deletedTag != null){
        //         //show ok
        //         return RedirectToAction("List");

        //     }
        //     else{
        //         //show error
        //     }

            
        //     return RedirectToAction("List");

        // }

    }
}