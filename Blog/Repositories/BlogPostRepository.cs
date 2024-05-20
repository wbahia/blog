using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories 
{
    public class BlogPostRepository : IBlogPostRepository
    {
        public BlogDbContext BlogDbContext { get; }
        
        public BlogPostRepository(BlogDbContext blogDbContext)
        {
            this.BlogDbContext = blogDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await this.BlogDbContext.AddAsync(blogPost);
            await this.BlogDbContext.SaveChangesAsync();
            return blogPost;
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await this.BlogDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await this.BlogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await this.BlogDbContext.BlogPosts.Include(t => t.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if(existingBlogPost != null){
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.Slug = blogPost.Slug;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.Tags = blogPost.Tags;
                existingBlogPost.Visible = blogPost.Visible;

                await this.BlogDbContext.SaveChangesAsync();

                return existingBlogPost;
            }

            return null;
        }
    }
}