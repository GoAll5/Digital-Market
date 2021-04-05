using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digital_Market.Models;
using Microsoft.AspNetCore.Identity;

namespace Digital_Market.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public AdminController(UserManager<AppUser> userManager,
                                IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
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
                    return RedirectToAction("Index");
                }

                foreach (IdentityError error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null) {
                return View(user);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string username, string password, 
            string email, string fullName, int age)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null) {
                if (!string.IsNullOrEmpty(email)) {
                    user.Email = email;
                } else {
                    ModelState.AddModelError("", "Email cannot be empty!");
                }

                if (!string.IsNullOrEmpty(password)) {
                    user.PasswordHash = _passwordHasher.HashPassword(user, password);
                } else { 
                    ModelState.AddModelError("", "Password cannot be empty!");
                }

                if (!string.IsNullOrEmpty(username)) {
                    user.UserName = username;
                } else {
                    ModelState.AddModelError("", "Username cannot be empty!");
                }

                if (!string.IsNullOrEmpty(fullName)) {
                    user.FullName = fullName;
                } else {
                    ModelState.AddModelError("", "Full name cannot be empty!");
                }

                if (age > 10 && age < 100) {
                    user.Age = age;
                } else {
                    ModelState.AddModelError("", "Age is not true!");
                }

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) &&
                    !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(fullName) && age > 10 && age < 100) {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded) {
                        return RedirectToAction("Index");
                    }

                    foreach (IdentityError error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            } else {
                ModelState.AddModelError("", "User not found!");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null) {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                } else {
                    foreach (IdentityError error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            } else {
                ModelState.AddModelError("", "User not found!");
            }

            return View("Index", _userManager.Users);
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
    }
}
