using Blog.Data;
using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public TagRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }
        
        public async Task<Tag> AddAsync(Tag tag)
        {
            await _blogDbContext.Tags.AddAsync(tag);
            await _blogDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tag = await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if(tag != null){
                _blogDbContext.Tags.Remove(tag);
                await _blogDbContext.SaveChangesAsync();

                return tag;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _blogDbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag =  await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == tag.Id);
            if(existingTag != null){
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                
                await _blogDbContext.SaveChangesAsync();
                return existingTag;
            }

            return null;
        }
    }
}