using System.Threading.Tasks;
using IdentityApp.DTO;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index() => View(await _manager.Users.ToListAsync());
        
        [HttpGet]
        public IActionResult Register() => View();

        [HttpGet]
        public IActionResult Login() => View();

        [HttpGet]
        public IActionResult ChangePassword(string id) => View((object) id);

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _manager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            EditUserDto dto = new EditUserDto
            {
                ID = user.Id,
                Email = user.Email,
                Name = user.Name
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            User user = await _manager.FindByIdAsync(dto.ID);

            if (user == null) return View(dto);
            
            user.Email = dto.Email;
            user.Name = dto.Name;

            var result = await _manager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(dto);
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

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            User user = await _manager.FindByIdAsync(dto.ID);

            if (user == null) return View(dto);

            var result = await _manager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
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

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _manager.FindByIdAsync(id);

            if (user != null) await _manager.DeleteAsync(user);

            return RedirectToAction("Index");
        }
    }
}