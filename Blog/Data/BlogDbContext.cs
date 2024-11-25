using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) :base(options)
        {
            
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<BlogPost>().HasMany(t => t.Tags).WithMany(t => t.BlogPosts);
        //}
    }
}