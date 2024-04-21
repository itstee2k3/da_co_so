using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using do_an_ltweb.Services;
using Microsoft.AspNetCore.Authorization;

namespace do_an.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IVnPayService _vnPayservice;

        public OrderController(ApplicationDbContext context, IVnPayService vnPayservice)
        {
            _context = context;
            _vnPayservice = vnPayservice;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MakePurchase(string paymentMethod)
        {
            // Lấy người dùng hiện tại (hoặc thông tin người dùng đang đăng nhập)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Lấy giá trị totalBill từ session
            byte[] totalBillBytes = HttpContext.Session.Get("TotalBill");
            double totalBillDouble = BitConverter.ToDouble(totalBillBytes, 0);
            decimal totalBill = Convert.ToDecimal(totalBillDouble);

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

            // Kiểm tra xem số lượng của mỗi mục trong giỏ hàng có nhỏ hơn hoặc bằng số lượng có sẵn của sản phẩm không
            List<string> invalidItems = new List<string>();
            foreach (var cartItem in cartItems)
            {
                var product = _context.Products.FirstOrDefault(p => p.IdProduct == cartItem.IdProduct);
                if (product == null || cartItem.Quantity > product.Nums)
                {
                    // Nếu số lượng mục vượt quá số lượng có sẵn của sản phẩm, thêm thông tin chi tiết vào danh sách invalidItems
                    if (product != null)
                    {
                        // Hiển thị tên sản phẩm và số lượng còn lại trong danh sách invalidItems
                        invalidItems.Add($"Product: {product.NameProduct}, Available Quantity: {product.Nums}");
                    }
                    else
                    {
                        invalidItems.Add($"Unknown product, Available Quantity: 0");
                    }
                }
            }

            if (invalidItems.Any())
            {
                // Nếu danh sách invalidItems không rỗng, trả về một thông báo JSON với thông tin chi tiết
                return Json(new { success = false, message = "The quantity of some items in your cart exceeds the available stock.", invalidItems = invalidItems });
            }

            if (paymentMethod == "vnPay")
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = totalBill,
                    CreatedDate = DateTime.Now,
                    Description = $"A á ớ",
                    FullName = "AAA",
                    OrderId = new Random().Next(1000, 100000)
                };
                //return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
                // Generate the VNPay payment URL
                var redirectUrl = _vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel);

                // Return the redirection URL in the JSON response
                return Json(new { success = true, redirectUrl = redirectUrl });
            }

            // Tạo một đơn hàng mới
            var order = new Order
            {
                DateBegin = DateTime.Now,
                IdUser = userId,
                Address = user.Address, // Gán địa chỉ của người dùng cho đơn hàng
                TotalBill = totalBill,
                StatusAdmin = 0, // Thực hiện cài đặt cho trạng thái đơn hàng tùy thuộc vào yêu cầu của ứng dụng của bạn
                StatusUser = 0, // Thực hiện cài đặt cho trạng thái đơn hàng tùy thuộc vào yêu cầu của ứng dụng của bạn
                PaymentMethod = 0
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

        [HttpPost]
        public IActionResult SaveTotalBill(decimal totalBill)
        {
            // Chuyển đổi giá trị decimal thành một mảng byte
            double totalBillDouble = Convert.ToDouble(totalBill);
            byte[] totalBillBytes = BitConverter.GetBytes(totalBillDouble);

            // Lưu giá trị totalBill dưới dạng mảng byte vào session
            HttpContext.Session.Set("TotalBill", totalBillBytes);

            return Ok();
        }

        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View("Success");
        }

        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }

        [Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            // Lưu đơn hàng vô database
            // Lấy người dùng hiện tại (hoặc thông tin người dùng đang đăng nhập)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Lấy giá trị totalBill từ session
            byte[] totalBillBytes = HttpContext.Session.Get("TotalBill");
            double totalBillDouble = BitConverter.ToDouble(totalBillBytes, 0);
            decimal totalBill = Convert.ToDecimal(totalBillDouble);

            // Lấy thông tin người dùng
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            // Lấy các mục trong giỏ hàng của người dùng
            var cartItems = _context.CartItems.Where(c => c.IdUser == userId).ToList();

            // Tạo một đơn hàng mới
            var order = new Order
            {
                DateBegin = DateTime.Now,
                IdUser = userId,
                Address = user.Address, // Gán địa chỉ của người dùng cho đơn hàng
                TotalBill = totalBill,
                StatusAdmin = 0, // Thực hiện cài đặt cho trạng thái đơn hàng tùy thuộc vào yêu cầu của ứng dụng của bạn
                StatusUser = 0, // Thực hiện cài đặt cho trạng thái đơn hàng tùy thuộc vào yêu cầu của ứng dụng của bạn
                PaymentMethod = 1
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

            TempData["Message"] = $"Thanh toán VNPay thành công";
            return RedirectToAction("PaymentSuccess");
        }
    }
}

