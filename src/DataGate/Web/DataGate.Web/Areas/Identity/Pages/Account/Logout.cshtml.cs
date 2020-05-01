﻿namespace DataGate.Web.Areas.Identity.Pages.Account
{
    using DataGate.Data.Models.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private const string LoginPageRoute = "/Identity/Account/Login";
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            if (this.User?.Identity.IsAuthenticated == true)
            {
                await this.signInManager.SignOutAsync();
                this.logger.LogInformation("User logged out.");
                return this.RedirectToPage();
            }

            return this.Redirect(LoginPageRoute);
        }
    }
}
