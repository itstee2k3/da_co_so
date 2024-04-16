using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryBrand
{
    [Authorize(Roles = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CategoryBrand Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? brandid)
        {
            if (brandid == null)
            {
                return NotFound("No brand ID specified");
            }

            Category = await _context.CategoryBrands.FindAsync(brandid);

            if (Category == null)
            {
                return NotFound("Brand not found");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? brandid)
        {
            if (brandid == null)
            {
                return NotFound("No brand ID specified");
            }

            Category = await _context.CategoryBrands.FindAsync(brandid);

            if (Category == null)
            {
                return NotFound("Brand not found");
            }

            _context.CategoryBrands.Remove(Category);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just deleted the brand: {Category.NameCategory}";

            return RedirectToPage("./Index");
        }
    }
}
