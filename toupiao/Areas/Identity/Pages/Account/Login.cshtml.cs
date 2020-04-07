using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using toupiao.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using toupiao.Data;
using Microsoft.EntityFrameworkCore;

namespace toupiao.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<Program> _localizer;
        private readonly ApplicationDbContext _context;

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            IConfiguration configuration ,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IStringLocalizer<Program> localizer,
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _localizer = localizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name ="邮箱")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name ="密码")]
            public string Password { get; set; }

            [Display(Name = "记住我?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {

                var _user = await _userManager.FindByEmailAsync(Input.Email);
                if (_user == null)
                {
                    return RedirectToPage("Register", new { IsFromLogin = true, returnUrl });
                }
                if (!await _userManager.IsEmailConfirmedAsync(_user))
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(_user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = _user.Id, code = code },
                        protocol: Request.Scheme);
                    
                    await ToupiaoEmailSender.SendEmailAnync(
                        _configuration,
                        Input.Email, 
                        _localizer["Confirm your email"],
                        _localizer["Please confirm your account by"]+" <a href='" + HtmlEncoder.Default.Encode(callbackUrl) +"'>"+_localizer["clicking here"]+"</a>");
                    ModelState.AddModelError(string.Empty, _localizer["We have sent a confirmation email to you, you can login after confirming it"]);
                    return Page();
                }


                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    if( _context.Users.Count() == 1)
                    {
                        // 第一个用户为管理员
                        if (!await _context.Roles.AnyAsync( 
                            p => p.Name == "ADMIN"))
                        {
                            var role = new IdentityRole
                            {
                                Name = "ADMIN",
                                NormalizedName = "ADMIN"
                            };

                            await _roleManager.CreateAsync(role);

                            await _context.SaveChangesAsync();


                        }

                        if(!User.IsInRole("ADMIN"))
                        {

                            await _userManager.AddToRolesAsync(
                                _user, new[] { "ADMIN" });

                            await _context.SaveChangesAsync();

                            _logger.LogInformation(
                                $"User: {_user.Email} is ADMIN now");
                        }

                    }

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
