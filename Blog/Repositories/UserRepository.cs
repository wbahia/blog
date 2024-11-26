using Blog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext context;

        public UserRepository(AuthDbContext context)
        {
            this.context = context;
        }
        
        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            //var users = await this.context.Users.Where(x => x.Email != "superadmin@bloggynho.com").ToListAsync();
            var users = await this.context.Users.ToListAsync();
            return users;
        }
    }
}
