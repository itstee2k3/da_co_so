using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Hosting;

namespace do_an_ltweb.Admin.AdProduct
{
    [Authorize(Roles = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DeleteModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? productid)
        {
            if (productid == null)
            {
                return NotFound("No product ID specified");
            }

            Product = await _context.Products.FindAsync(productid);

            if (Product == null)
            {
                return NotFound("Product not found");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? productid)
        {
            if (productid == null)
            {
                return NotFound("No product ID specified");
            }

            Product = await _context.Products.FindAsync(productid);

            if (Product == null)
            {
                return NotFound("Product not found");
            }

            // Xác định đường dẫn của ảnh sản phẩm cần xoá
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, Product.ImageUrl.TrimStart('/'));

            // Xoá ảnh nếu tồn tại
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just deleted the product: {Product.NameProduct}";

            return RedirectToPage("./Index");
        }
    }
}
