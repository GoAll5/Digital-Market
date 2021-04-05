using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digital_Market.Domain;
using Digital_Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Digital_Market.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        
        private readonly AppDbContext _dbContext;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signManager,
            IPasswordHasher<AppUser> passwordHasher,
            AppDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signManager;
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            AppUser user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user);
        }

        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new UserModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (ModelState.IsValid) {
                AppUser appUser = new AppUser {
                    UserName = user.Username,
                    FullName = user.FullName,
                    Age = user.Age,
                    Email = user.EMail
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded) {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            AppUser user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null) {
                await _signInManager.SignOutAsync();
                var result =
                    await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded) {
                    return RedirectToAction("Index", "Account");
                }
                ModelState.AddModelError("", "Wrong username or password!");
            }
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
