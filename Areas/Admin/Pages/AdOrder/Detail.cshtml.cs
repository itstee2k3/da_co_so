using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using do_an_ltweb.Models;
using do_an_ltweb.Admin.AdRole;
using Newtonsoft.Json;

namespace do_an_ltweb.Admin.AdOrder
{
    // Policy: Tạo ra các policy -> AllowEditRole
    [Authorize(Roles = "admin")]
    public class DetailModel : RolePageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : base(roleManager, context)
        {
            _context = context;
        }

        public class ProductDetail
        {
            public string ImageUrl { get; set; } // Thêm thuộc tính để lưu đường dẫn đến ảnh sản phẩm
            public string Name { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        [TempData]
        public string StatusMessage { get; set; }

        public int OrderId { get; set; }
        public List<ProductDetail> Products { get; set; }
        public Order Order { get; set; } // Thêm thuộc tính Order vào DetailModel

        public async Task<IActionResult> OnGet(int orderId)
        {
            // Lưu đối tượng đơn hàng vào session thay vì chỉ lưu orderId
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.IdOrder == orderId);

            if (orderId < 0)
                return NotFound("Order not found");

            if (order == null)
                return NotFound("Order not found");

            OrderId = orderId;

            Order = order;
            // Lấy thông tin chi tiết sản phẩm từ đơn hàng
            Products = await _context.OrderDetails
                .Where(oi => oi.IdOrder == orderId)
                .Select(oi => new ProductDetail
                {
                    ImageUrl = oi.Product.ImageUrl,
                    Name = oi.Product.NameProduct,
                    Quantity = oi.Quantity,
                    Price = oi.Product.Price
                })
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmOrder(int orderId)
        {

            var orderFromDb = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.IdOrder == orderId);

            if (orderFromDb == null)
                return NotFound("Order not found");

            // Duyệt qua từng chi tiết đơn hàng
            foreach (var orderDetail in orderFromDb.OrderDetails)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.IdProduct == orderDetail.IdProduct);

                if (product == null)
                {
                    return NotFound("No products found in stock");
                }

                // Kiểm tra số lượng sản phẩm trong kho
                if (product.Nums < orderDetail.Quantity)
                {

                    StatusMessage = $"Not enough stock for product: {product.NameProduct}";
                    return RedirectToPage("./Index");
                }

                // Cập nhật số lượng sản phẩm trong kho
                //product.Nums -= orderDetail.Quantity;
            }

            // Gán DateEnd của đơn hàng bằng DateTime.Now
            //orderFromDb.DateEnd = DateTime.Now;

            // Thực hiện xác nhận đơn hàng
            orderFromDb.StatusAdmin = 1; // Giả sử 1 là mã trạng thái cho đơn hàng đã được xác nhận

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            if (orderFromDb != null)
            {
                HttpContext.Session.Remove("OrderData");

                StatusMessage = $"You just confirm IdOrder: {orderFromDb.IdOrder}";
            }
            else
            {
                StatusMessage = "Order not found";
            }
            return RedirectToPage("./Index");
        }
    }
}
