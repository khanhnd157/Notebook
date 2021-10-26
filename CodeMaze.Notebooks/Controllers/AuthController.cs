using CodeMaze.Models;
using CodeMaze.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodeMaze.Notebooks.Controllers
{

    [AllowAnonymous]
    public class AuthController : Controller
    {

        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (HttpContext.User.Identity.IsAuthenticated) return Redirect("/");

            return View("Sign-In");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/auth/signin"), Route("/auth/signin.html")]
        public async Task<IActionResult> SignInAsync(UserModel account, string callback = "")
        {
            if (HttpContext.User.Identity.IsAuthenticated) return Redirect("/");

            var user = await _userService.SignInAsync(account.Username, account.Password);

            if (user?.IsActive == true)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimUserTypes.UserName, user.Username),
                    new Claim(ClaimUserTypes.Email, user.Email),
                    new Claim(ClaimUserTypes.Avatar, user.Avatar??string.Empty),
                    new Claim(ClaimUserTypes.DisplayName, user.DisplayName),
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieNotebook");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-5.0#create-an-authentication-cookie
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                if (!string.IsNullOrWhiteSpace(callback))
                    return Redirect(callback.Trim());

                return Redirect("/notes/manage");
            }

            return View("Sign-In");
        }


        [HttpPost]
        [Route("/auth/signup")]
        public async Task<IActionResult> SignUpAsync(UserModel user)
        {
            var result = await _userService.RegisterAsync(user);

            if (result is not null)
            {
                return Redirect($"/auth/signin");
            }

            return View("Sign-Up");
        }


        [HttpGet]
        [Route("/auth/signup")]
        public IActionResult SignUp()
        {
            return View("Sign-Up");
        }

        [HttpGet]
        [Route("/auth/signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
