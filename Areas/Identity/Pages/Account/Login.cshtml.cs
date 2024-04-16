using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace do_an.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger, UserManager<User> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Not null")]
            [Display(Name = "Username or Email")]
            [StringLength(100, MinimumLength = 1, ErrorMessage = "Enter correct information")]
            public string UserNameOrEmail { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Thực hiện xác thực đăng nhập và xử lý nếu không có lỗi nào được thêm vào ModelState
                // Nếu có lỗi, ModelState.IsValid sẽ trả về false và lỗi sẽ được hiển thị trên giao diện người dùng
                var result = await _signInManager.PasswordSignInAsync(
                    Input.UserNameOrEmail,
                    Input.Password,
                    Input.RememberMe,
                    lockoutOnFailure: true);

                if (!result.Succeeded)
                {
                    // Thất bại username/password -> tìm user theo email, nếu thấy thì thử đăng nhập
                    // bằng user tìm được
                    var user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);
                    if (user != null)
                    {
                        result = await _signInManager.PasswordSignInAsync(
                            user.UserName,
                            Input.Password,
                            Input.RememberMe,
                            true
                        );
                    }
                }

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    // Kiểm tra xem người dùng có vai trò "admin" không
                    var user = await _userManager.FindByNameAsync(Input.UserNameOrEmail);
                    if (await _userManager.IsInRoleAsync(user, "admin"))
                    {
                        // Nếu có, chuyển hướng trực tiếp đến trang Admin
                        return LocalRedirect("/admin/index");
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

            // Nếu ModelState.IsValid trả về false, quay trở lại trang đăng nhập để hiển thị lỗi
            return Page();
        }
    }
}
