using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LabPro.Web.Models;
using LabPro.Web.Authentication;

namespace LabPro.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IWebHostEnvironment env;

        public AccountController(IWebHostEnvironment env, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            string userName = userLogin.UserName;
            string password = userLogin.Password;

            if (env.EnvironmentName == "Development" && userName == "admin" && password == "admin")
            {
                var claims = new List<Claim>()
                {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Email, "admin")
                };

                roleManager.Roles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r.Name)));
                await signInManager.SignInWithClaimsAsync(new ApplicationUser { UserName = userName, Email = userName }, isPersistent: false, claims);

                return Redirect("~/");
            }

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {

                var result = await signInManager.PasswordSignInAsync(userName, password, false, false);

                if (result.Succeeded)
                {
                    return Redirect("~/");
                }
            }

            return Redirect("~/Login?error=Invalid user or password");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Redirect("~/");
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return Redirect("~/Login?error=Invalid user or password");
            }

            var user = new ApplicationUser { UserName = userName, Email = userName };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("~/");
            }

            var message = string.Join(", ", result.Errors.Select(error => error.Description));

            return Redirect($"~/Login?error={message}");
        }
    }
}
