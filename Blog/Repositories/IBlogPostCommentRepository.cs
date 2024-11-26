using Blog.Models.Domain;

namespace Blog.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment comment);
        Task<IEnumerable<BlogPostComment>> GetAllByBlogIdAsync(Guid blogPostId);
    }
}
