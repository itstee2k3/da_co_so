using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryPrice
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
        public CategoryPrice Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? priceid)
        {
            if (priceid == null)
            {
                return NotFound("No price ID specified");
            }

            Category = await _context.CategoryPrices.FindAsync(priceid);

            if (Category == null)
            {
                return NotFound("Price not found");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? priceid)
        {
            if (priceid == null)
            {
                return NotFound("No price ID specified");
            }

            Category = await _context.CategoryPrices.FindAsync(priceid);

            if (Category == null)
            {
                return NotFound("Price not found");
            }

            _context.CategoryPrices.Remove(Category);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just deleted the price: {Category.NameCategory}";

            return RedirectToPage("./Index");
        }
    }
}
