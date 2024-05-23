using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //seed roles (user, admin, superadmin)
            
            var adminRoleId = Guid.NewGuid().ToString();
            var superAdminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            var roles = new List<IdentityRole>{
                new IdentityRole {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //seed superadmin

            var superAdminId = Guid.NewGuid().ToString();
            var superAdminUser = new IdentityUser{
                UserName = "superadmin@bloggynho.com",
                Email = "superadmin@bloggynho.com",
                NormalizedEmail = "superadmin@bloggynho.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggynho.com".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "Super@1234");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //add all roles to superadmin

            var superAdminRoles = new List<IdentityUserRole<string>>{
                new() {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new() {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new() {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);


        }
    }
}