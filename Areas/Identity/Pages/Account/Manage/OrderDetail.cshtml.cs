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

namespace do_an.Areas.Identity.Pages.Account.Manage
{
    // Policy: Tạo ra các policy -> AllowEditRole
    [Authorize]
    public class DetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public class ProductDetail
        {
            public string ImageUrl { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        [TempData]
        public string StatusMessage { get; set; }

        public Order Order { get; set; } // Stores the order details

        // Retrieves order details based on orderId passed as parameter
        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            Order = await _context.Orders
                                   .Include(o => o.OrderDetails)
                                   .ThenInclude(od => od.Product)
                                   .FirstOrDefaultAsync(o => o.IdOrder == orderId);

            if (Order == null)
            {
                StatusMessage = "Order not found";
                return NotFound(StatusMessage);
            }

            // Converts order details to ProductDetail list
            Products = Order.OrderDetails.Select(od => new ProductDetail
            {
                ImageUrl = od.Product.ImageUrl,
                Name = od.Product.NameProduct,
                Quantity = od.Quantity,
                Price = od.Product.Price
            }).ToList();

            return Page();
        }

        public List<ProductDetail> Products { get; set; } // List to store product details for the UI
    }
}
