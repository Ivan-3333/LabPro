using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Authorization;
using LabPro.Web.Models;

namespace LabPro.Web.Services
{
    public class SecurityService
    {
        public event Action Authenticated;

        private readonly IWebHostEnvironment env;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly NavigationManager uriHelper;

        public SecurityService(
            IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            NavigationManager uriHelper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.env = env;
            this.uriHelper = uriHelper;
        }


        ApplicationUser user;
        public ApplicationUser User
        {
            get
            {
                if (user == null)
                {
                    return new ApplicationUser() { Name = "Anonymous" };
                }

                return user;
            }
        }

        public ClaimsPrincipal Principal { get; set; }


        public async Task<bool> InitializeAsync(AuthenticationStateProvider authenticationStateProvider)
        {
            var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();
            Principal = authenticationState.User;

            var name = Principal.Identity.Name;

            if (env.EnvironmentName == "Development" && name == "admin")
            {
                user = new ApplicationUser { UserName = name };
            }

            if (user == null && name != null)
            {
                
            }

            var result = IsAuthenticated();
            if (result)
            {
                Authenticated?.Invoke();
            }

            return result;
        }


        public bool IsAuthenticated()
        {
            return Principal != null ? Principal.Identity.IsAuthenticated : false;
        }
    }
}
