using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using do_an.Models;  // Import your Models namespace
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace do_an.Areas.Identity.Pages.Account.Manage
{
    public class TwoFactorAuthenticationPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TwoFactorAuthenticationPageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Product> FavoriteProducts { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Lấy danh sách sản phẩm yêu thích của người dùng từ bảng Favourites
            FavoriteProducts = await _context.Favourites
                .Where(f => f.IdUser == userId)  // Sử dụng Id của người dùng để lọc
                .Include(f => f.Product)  // Liên kết với bảng Product để lấy thông tin chi tiết sản phẩm
                .Select(f => f.Product)  // Chọn sản phẩm từ bảng Product
                .ToListAsync();

            return Page();
        }
    }
}
