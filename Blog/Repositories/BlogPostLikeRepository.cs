
using Blog.Data;
using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BlogDbContext Context;

        public BlogPostLikeRepository(BlogDbContext context)
        {
            Context = context;
        }

        public async Task<BlogPostLike> AddLike(BlogPostLike blogPostLike)
        {
            await Context.BlogPostLike.AddAsync(blogPostLike);
            await Context.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikes(Guid blogPostId)
        {
            return await Context.BlogPostLike.Where(x => x.BlogPostId == blogPostId)
                        .ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await Context.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
        }   
    }
}
