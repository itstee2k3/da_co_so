using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace do_an_ltweb.Admin.AdUser
{
    [Authorize(Roles = "admin")]
    public class LockModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public LockModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public User user { get; set; }

        [BindProperty]
        [DataType(DataType.DateTime)]
        public DateTime LockoutDateTime { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("User no found.");
            }

            user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"User with ID not found '{id}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLockAsync(string id)
        {
            user = await _userManager.FindByIdAsync(id);

            if (string.IsNullOrEmpty(id) || user == null)
            {
                return NotFound("User no found");
            }

            if (!await _userManager.IsLockedOutAsync(user))
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
            }

            if (LockoutDateTime == default)
            {
                // Đặt giá trị mặc định cho LockoutDateTime là thời điểm hiện tại (Việt Nam)
                LockoutDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            }

            // Gắn LockoutEnd cho user
            user.LockoutEnd = LockoutDateTime;

            var lockoutResult = await _userManager.SetLockoutEndDateAsync(user, LockoutDateTime);

            if (!lockoutResult.Succeeded)
            {
                // Xử lý lỗi nếu cần thiết
                // Ví dụ: return BadRequest();
                return BadRequest("Unable to lock user.");
            }

            StatusMessage = $"The account of user '{user.UserName}' has been locked to {LockoutDateTime.ToString("dd/MM/yyyy HH:mm:ss")} in Vietnam time zone.";

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostUnlockAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null); // Đặt thời gian mở khoá thành null để mở khoá tài khoản
            if (result.Succeeded)
            {
                StatusMessage = $"User '{user.UserName}' account unlocked";

                return RedirectToPage("./Index"); // Chuyển hướng về trang danh sách thành viên sau khi mở khoá thành công
            }
            else
            {
                // Nếu có lỗi xảy ra, hiển thị thông báo lỗi
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Page();
            }
        }
    }
}


