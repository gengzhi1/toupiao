using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using toupiao.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;

namespace toupiao.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<Program> _localizer;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;


        public HomeController(
            ILogger<HomeController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
        IStringLocalizer<Program> localizer)
        {
            _logger = logger;
            _localizer = localizer;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            Response.Cookies.Append(
                key: "IsAdmin",
                value: "1",
                options: new CookieOptions()
                {
                    Expires = DateTime.Now.AddYears(
                        User.IsInRole("ADMIN")? 1:-1)
                });


            ViewData["TEST"] = _localizer["TEST"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
