// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace do_an.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class OrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        [TempData]
        public string StatusMessage { get; set; }

        public OrderModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                Orders = await _context.Orders
                                        .Where(o => o.IdUser == currentUser.Id)
                                        .ToListAsync();
            }
        }

        // Handler for requesting cancellation
        public async Task<IActionResult> OnPostRequestCancellationAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                TempData["Message"] = "Order not found.";
                return RedirectToPage();
            }

            order.StatusUser = -1; // Status for request cancellation
            await _context.SaveChangesAsync();
            TempData["Message"] = "Cancellation request submitted successfully.";
            return RedirectToPage();
        }

        // Handler for cancelling the cancellation request
        public async Task<IActionResult> OnPostCancelCancellationRequestAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                TempData["Message"] = "Order not found.";
                return RedirectToPage();
            }

            order.StatusUser = 0; // Reset status
            await _context.SaveChangesAsync();
            TempData["Message"] = "Cancellation request has been withdrawn.";
            return RedirectToPage();
        }

        // Handler for confirming goods received
        public async Task<IActionResult> OnPostConfirmGoodsReceivedAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                TempData["Message"] = "Order not found.";
                return RedirectToPage();
            }

            // Gán DateEnd của đơn hàng bằng DateTime.Now
            order.DateEnd = DateTime.Now;

            var orderFromDb = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.IdOrder == orderId);

            // Duyệt qua từng chi tiết đơn hàng
            foreach (var orderDetail in orderFromDb.OrderDetails)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.IdProduct == orderDetail.IdProduct);

                if (product == null)
                {
                    return NotFound("No products found");
                }

                // Kiểm tra số lượng sản phẩm trong kho
                //if (product.Nums < orderDetail.Quantity)
                //{

                //    StatusMessage = $"Not enough stock for product: {product.NameProduct}";
                //    return RedirectToPage("./Index");
                //}

                // Cập nhật số lượng sản phẩm trong kho
                product.Nums -= orderDetail.Quantity;
            }

            order.StatusUser = 1; // Status for goods received
            await _context.SaveChangesAsync();
            TempData["Message"] = "Receipt of goods confirmed successfully.";
            return RedirectToPage();
        }
    }

}
