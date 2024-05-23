using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Blog.Controllers
{
    
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> UserManager;
        private readonly SignInManager<IdentityUser> SignInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var identityUser = new IdentityUser{
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email
            };
            
            var identityResult = await UserManager.CreateAsync(identityUser, registerViewModel.Password);

            if(identityResult.Succeeded)
            {
                //add user role
                var roleIdentityResult = await UserManager.AddToRoleAsync(identityUser, "User");

                if(roleIdentityResult.Succeeded){
                    //show success
                    return RedirectToAction("Register");
                }
            }

            //show error
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            var signInResult = await SignInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);

            if(signInResult != null && signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }


            //show error
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}