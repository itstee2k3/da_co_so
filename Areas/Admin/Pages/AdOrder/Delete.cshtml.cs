using System.Data;
using System.Threading.Tasks;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace do_an_ltweb.Admin.AdOrder
{
    [Authorize(Roles = "admin")]
    public class DelateOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context; // Thay YourDbContext bằng tên DbContext của ứng dụng của bạn

        public DelateOrderModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } // Thay Order bằng tên lớp model của order trong ứng dụng của bạn

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            Order = await _context.Orders.FindAsync(orderId);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            Order = await _context.Orders.FindAsync(orderId);

            if (Order != null)
            {
                _context.Orders.Remove(Order);
                await _context.SaveChangesAsync();
            }

            StatusMessage = $"You just deleted IdOrder: {Order.IdOrder}";
            return RedirectToPage("./Index"); // Chuyển hướng sau khi xóa thành công, có thể thay đổi đường dẫn tùy ý
        }
    }
}
