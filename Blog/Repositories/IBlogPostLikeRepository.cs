using Blog.Models.Domain;
using Blog.Models.ViewModels;

namespace Blog.Repositories
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid blogPostId);
        Task<IEnumerable<BlogPostLike>> GetLikes(Guid blogPostId);
        Task<BlogPostLike> AddLike(BlogPostLike blogPostLike);
    }
}
