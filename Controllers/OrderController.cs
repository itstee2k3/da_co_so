using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace do_an.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult MakePurchase()
        {
            // Lấy người dùng hiện tại (hoặc thông tin người dùng đang đăng nhập)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Lấy các mục trong giỏ hàng của người dùng
            var cartItems = _context.CartItems.Where(c => c.IdUser == userId).ToList();

            if (cartItems.Count == 0)
            {
                // Nếu không có sản phẩm trong giỏ hàng, trả về một thông báo JSON yêu cầu thêm sản phẩm vào giỏ hàng
                return Json(new { success = false, message = "Please add items to your cart before placing an order." });
            }

            // Lấy thông tin người dùng
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            // Kiểm tra xem địa chỉ của người dùng có null không
            if (user.Address == null)
            {
                // Nếu địa chỉ là null, trả về một thông báo JSON yêu cầu người dùng cập nhật địa chỉ
                return Json(new { success = false, message = "Please update your address before ordering." });
            }

            // Tạo một đơn hàng mới
            var order = new Order
            {
                DateBegin = DateTime.Now,
                IdUser = userId,
                Address = user.Address, // Gán địa chỉ của người dùng cho đơn hàng
                Status = 0 // Thực hiện cài đặt cho trạng thái đơn hàng tùy thuộc vào yêu cầu của ứng dụng của bạn
            };

            // Thêm các sản phẩm từ giỏ hàng vào đơn hàng
            foreach (var cartItem in cartItems)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    IdProduct = cartItem.IdProduct,
                    Quantity = cartItem.Quantity
                });
            }

            // Lưu đơn hàng vào cơ sở dữ liệu
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Xóa các mục đã chuyển sang đơn hàng khỏi giỏ hàng
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();

            return Json(new { success = true, message = "Order created successfully." });
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}

