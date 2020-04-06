using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using toupiao.Services;

namespace toupiao.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IConfiguration _configuration ;
        private readonly IStringLocalizer<Program> _localizer;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration , 
            IStringLocalizer<Program> localizer,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _localizer = localizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage="必须填写邮箱")]
            [EmailAddress(ErrorMessage="你还输入的不是邮箱")]
            [Display(Name = "电子邮件")]
            public string Email { get; set; }

            [Required(ErrorMessage="请输入密码")]
            [StringLength(100, 
                ErrorMessage = "{0}最小为{2}个字符，最多为{1}个字符。", 
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            [RegularExpression(
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&_])"+
                @"[A-Za-z\d$@$!%*?&_]{6,}",
                ErrorMessage = "密码要有数字大小写和字符(@$!%*?&_), 6位以上哦!")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "再次确认密码")]
            [Compare("Password", ErrorMessage = "两次密码不一样")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager
                .GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager
                .GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { 
                    UserName = Input.Email, 
                    Email = Input.Email };
                    
                var result = await _userManager.CreateAsync(
                        user, 
                        Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation(
                            "User created a new account with password.");

                    var code = await _userManager
                        .GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(
                        Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { 
                            area = "Identity", 
                            userId = user.Id, 
                            code = code },
                        protocol: Request.Scheme);

                    await ToupiaoEmailSender.SendEmailAnync( 
                        _configuration, 
                        Input.Email, 
                        _localizer["Confirm your email"],
                        _localizer[$"Please confirm your account by"]+
                        $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>"
                            +_localizer["clicking here"]+
                        "</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
