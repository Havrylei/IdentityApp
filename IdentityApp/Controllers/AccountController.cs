using System.Threading.Tasks;
using IdentityApp.DTO;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _manager;
        private readonly SignInManager<User> _sign;

        public AccountController(UserManager<User> manager, SignInManager<User> sign)
        {
            _manager = manager;
            _sign = sign;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Content(User.Identity.Name);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = dto.Email,
                    UserName = dto.Login,
                    Name = dto.Name
                };

                var result = await _manager.CreateAsync(user, dto.Password);

                if (result.Succeeded)
                {
                    await _sign.SignInAsync(user, false);

                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(dto);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _sign.PasswordSignInAsync(dto.Login, dto.Password, dto.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Wrong login or password.");
                }
            }

            return View(dto);
        }

        [HttpDelete]
        public async Task<IActionResult> LogOut()
        {
            await _sign.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}