using do_an_ltweb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging; // Import thêm namespace này
using System.Collections.Generic; // Import thêm namespace này
using Microsoft.AspNetCore.Http; // Thêm namespace này
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Authorization;
using do_an_ltweb.Services;

namespace do_an.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _user;
        private readonly IVnPayService _vnPayservice;

        public CartController(ILogger<CartController> logger, ApplicationDbContext context, UserManager<User> user, IVnPayService vnPayservice)
        {
            _logger = logger;
            _context = context;
            _user = user;
            _vnPayservice = vnPayservice;
        }

        public async Task<IActionResult> Index() // Đánh dấu phương thức này là async
        {
            var user = await _user.GetUserAsync(User);
            if (user == null)
            {
                var returnUrl = HttpContext.Request.Path;
                return Redirect($"/login?returnUrl={returnUrl}");
            }

            // Lấy danh sách các sản phẩm trong giỏ hàng của người dùng
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.CategoryFrameColor) // Bao gồm thông tin về màu khung của sản phẩm

                .Where(ci => ci.IdUser == user.Id)
                .ToListAsync();

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity) // Đánh dấu phương thức này là async
        {
            // Lấy thông tin người dùng hiện tại
            var user = await _user.GetUserAsync(User);

            // Kiểm tra xem người dùng có đăng nhập không
            if (user == null)
            {
                return Json(new { error = "User is not logged in" });
            }

            // Kiểm tra xem sản phẩm có tồn tại không
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return Json(new { error = "Product not found" });
            }

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.IdProduct == productId && ci.IdUser == user.Id);

            if (existingCartItem != null)
            {
                // Nếu sản phẩm đã tồn tại trong giỏ hàng, cập nhật số lượng
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Nếu sản phẩm chưa có trong giỏ hàng, tạo mới một mục trong giỏ hàng
                var cartItem = new CartItem
                {
                    IdUser = user.Id,
                    IdProduct = productId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
            {
                return NotFound(new { success = false, error = "Không tìm thấy mục trong giỏ hàng" });
            }

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }



        [HttpPost]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
            {
                return NotFound(new { success = false, error = "Không tìm thấy mục trong giỏ hàng" });
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
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

        //[Authorize]
        //public IActionResult PaymentCallBack()
        //{
        //    var response = _vnPayservice.PaymentExecute(Request.Query);

        //    if (response == null || response.VnPayResponseCode != "00")
        //    {
        //        TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
        //        return RedirectToAction("PaymentFail");
        //    }


        //    // Lưu đơn hàng vô database

        //    TempData["Message"] = $"Thanh toán VNPay thành công";
        //    return RedirectToAction("PaymentSuccess");
        //}
    }
}
        