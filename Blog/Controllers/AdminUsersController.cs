using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository repository;
        private readonly UserManager<IdentityUser> user;

        public AdminUsersController(IUserRepository repository, UserManager<IdentityUser> user)
        {
            this.repository = repository;
            this.user = user;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await this.repository.GetAllAsync();
            var listUsers = new List<User>();
            var userViewModel = new UserViewModel();
            foreach (var user in users)
            {
                listUsers.Add(new User
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    UserName = user.UserName
                });
            }
            userViewModel.Users = listUsers;
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email,
            };
            var result = await user.CreateAsync(identityUser, request.Password);
            if (result != null)
            {
                if (result.Succeeded)
                {
                    //assign roles
                    var roles = new List<string>
                    {
                        "User"
                    };
                    if (request.AdminRoleCheckbox)
                    {
                        roles.Add("Admin");
                    }

                    result = await user.AddToRolesAsync(identityUser, roles);
                    if (result != null && result.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUsers");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var usr = await user.FindByIdAsync(id.ToString());

            if (usr != null)
            {
                var result = await user.DeleteAsync(usr);
                if (result != null && result.Succeeded)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }

            return View();
        }
    }
}
