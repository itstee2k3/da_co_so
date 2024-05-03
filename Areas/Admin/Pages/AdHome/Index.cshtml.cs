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

namespace do_an_ltweb.Admin.AdHome
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<decimal> RevenueData { get; set; }

        public async Task<IActionResult> OnGetRevenueDataAsync()
        {
            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now;

            // Lấy ngày 6 tháng trước
            DateTime sixMonthsAgo = currentDate.AddMonths(-5);

            //// Truy vấn cơ sở dữ liệu để lấy thông tin đơn hàng và chi tiết đơn hàng trong 6 tháng gần nhất
            //RevenueData = await _context.Orders
            //    .Where(o => o.DateEnd.HasValue && o.DateEnd.Value >= sixMonthsAgo && o.DateEnd.Value <= currentDate)
            //    .GroupBy(o => new { Year = o.DateEnd.Value.Year, Month = o.DateEnd.Value.Month })
            //    .Select(g => g.Sum(o => o.TotalBill))
            //    //.OrderByDescending(sum => sum)
            //    .Take(6)
            //    .ToListAsync();

            // Truy vấn cơ sở dữ liệu để lấy thông tin đơn hàng và chi tiết đơn hàng trong 6 tháng gần nhất
            var revenueData = await _context.Orders
                .Where(o => o.DateEnd.HasValue && o.DateEnd.Value >= sixMonthsAgo && o.DateEnd.Value <= currentDate)
                .GroupBy(o => new { Year = o.DateEnd.Value.Year, Month = o.DateEnd.Value.Month })
                .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Total = g.Sum(o => o.TotalBill) })
                .OrderBy(g => g.Year).ThenBy(g => g.Month) // Sắp xếp theo năm và tháng để đảm bảo thứ tự đúng
                .Take(6)
                .ToListAsync();

            // Chuyển đổi dữ liệu sang kiểu danh sách decimal
            //RevenueData = revenueData.Select(item => item.Total).ToList();

            return new JsonResult(revenueData);
        }
    }
}
