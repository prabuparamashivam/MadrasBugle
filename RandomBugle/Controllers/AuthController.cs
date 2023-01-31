using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandomBugle.Services.Email;
using RandomBugle.ViewModels;
using System.Threading.Tasks;

namespace RandomBugle.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private IEmailService _emailService;
        public  AuthController (SignInManager<IdentityUser> signInmanager,
                                UserManager<IdentityUser> userManager,
                                IEmailService emailService)
        {
            _signInManager = signInmanager;
            _userManager = userManager;
           _emailService = emailService;

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new ViewModels.LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(ViewModels.LoginViewModel vm)
        {

            var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
           if(!result.Succeeded)
            {
                return View(vm);
            }

            var user = await _userManager.FindByNameAsync(vm.UserName);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
            {
                return RedirectToAction("index", "Panel");
            }
            return RedirectToAction("Index","Home");

        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(ViewModels.RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = new IdentityUser
            {
                UserName = vm.Email,
                Email = vm.Email
            };
            var result = await _userManager.CreateAsync(user, "password");

            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _emailService.SendEmail(user.Email, "Welcome", "Thank you for registering !");
                return RedirectToAction("Index", "Home");
            }
             return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
