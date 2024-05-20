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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = await this.BlogPostRepository.GetAsync(id);
            var tagsDomainModel = await this.TagRepository.GetAllAsync();
            if(post != null){
                var model = new EditBlogPostRequest
                {
                    Id = post.Id,
                    PageTitle = post.PageTitle,
                    Heading = post.Heading,
                    Author = post.Author,
                    Content = post.Content,
                    PublishedDate = post.PublishedDate,
                    ShortDescription = post.ShortDescription,
                    Visible = post.Visible,
                    Slug = post.Slug,
                    FeaturedImageUrl = post.FeaturedImageUrl,
                    Tags = tagsDomainModel.Select(x => new SelectListItem{
                        Text = x.Name, Value = x.Id.ToString()
                    }),
                    SelectedTags = post.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var post = new BlogPost
            {
                    Id = editBlogPostRequest.Id,
                    PageTitle = editBlogPostRequest.PageTitle,
                    Heading = editBlogPostRequest.Heading,
                    Author = editBlogPostRequest.Author,
                    Content = editBlogPostRequest.Content,
                    PublishedDate = editBlogPostRequest.PublishedDate,
                    ShortDescription = editBlogPostRequest.ShortDescription,
                    Visible = editBlogPostRequest.Visible,
                    Slug = editBlogPostRequest.Slug,
                    FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in editBlogPostRequest.SelectedTags)
            {
                var existingTag = await TagRepository.GetAsync(Guid.Parse(selectedTagId));
                if (existingTag != null){
                    selectedTags.Add(existingTag);
                }
            }

            post.Tags = selectedTags;
            var updatedBlogPost = await this.BlogPostRepository.UpdateAsync(post);

            if(updatedBlogPost != null){
                //show ok

            }
            else{
                //show error
            }
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });

        }

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