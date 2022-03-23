using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using UmniahUsers_UI.Models;

namespace UmniahUsers_UI.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;


        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            HttpResponseMessage response = Helpers.WebApiHelper.webApiClient.GetAsync("User/GetAllUsers").Result;
            List<UserModel> users = response.Content.ReadAsAsync<List<UserModel>>().Result;
            return View(users);
        }

        [Authorize]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Register([Bind("UserName,FullName,Password,Mobile")] UserModel user)
        {
            var url = string.Format("User/GetUserByUserName/{0}", user.UserName);
            HttpResponseMessage response = Helpers.WebApiHelper.webApiClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                UserModel model = response.Content.ReadAsAsync<UserModel>().Result;
                if (model != null)
                {
                    ModelState.AddModelError(string.Empty, "Username is already taken!");
                }
            }
            user.CreationDate = DateTime.Now;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            //user.Password = PasswordHelper.HashPassword(user.Password);
            if (ModelState.IsValid)
            {
                HttpResponseMessage responseAdd = Helpers.WebApiHelper.webApiClient.PostAsJsonAsync("User/AddUser", user).Result;
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var url = string.Format("User/GetUserById/{0}", id);
            HttpResponseMessage response = Helpers.WebApiHelper.webApiClient.GetAsync(url).Result;
            UserModel user = response.Content.ReadAsAsync<UserModel>().Result;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit([Bind("Id,UserName,FullName,Mobile")] UserModel user)
        {
            if (user != null && user.Id != 0)
            {
                var url = string.Format("User/GetUserById/{0}", user.Id);
                HttpResponseMessage response = Helpers.WebApiHelper.webApiClient.GetAsync(url).Result;
                UserModel userModel = response.Content.ReadAsAsync<UserModel>().Result;
                user.Password = userModel.Password;
                user.CreationDate = userModel.CreationDate;
                user.IsActive = userModel.IsActive;
                HttpResponseMessage responseEdit = Helpers.WebApiHelper.webApiClient.PutAsJsonAsync("User/UpdateUser", user).Result;
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var url = string.Format("User/GetUserByUserName/{0}", username);
                HttpResponseMessage response = Helpers.WebApiHelper.webApiClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    UserModel user = response.Content.ReadAsAsync<UserModel>().Result;
                    if (user != null)
                    {
                        bool isVerified = BCrypt.Net.BCrypt.Verify(password, user.Password);
                        if (isVerified)
                        {
                            var claims = new List<Claim>
                          {
                              new Claim(ClaimTypes.Name, username),
                              new Claim("FullName", user.FullName),
                              new Claim(ClaimTypes.Role, "User"),
                          };

                            var claimsIdentity = new ClaimsIdentity(
                                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            var authProperties = new AuthenticationProperties
                            {
                                AllowRefresh = true,
                                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                                IsPersistent = true,
                            };

                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                authProperties);

                            _logger.LogInformation("User logged in");
                            return RedirectToAction("index");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid UserName/Password");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
