using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdOrder
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public List<Order> Orders { get; set; }

        public async Task OnGet()
        {
            Orders = await _context.Orders
                .Include(o => o.User) // Nạp thông tin người dùng liên quan
                .Include(o => o.OrderDetails) // Nạp thông tin chi tiết đơn hàng liên quan
                    .ThenInclude(od => od.Product) // Nạp thông tin sản phẩm liên quan thông qua bảng OrderDetail
                .OrderBy(p => p.IdOrder)
                .ToListAsync();
        }
        public void OnPost() => RedirectToPage();
    }
}
