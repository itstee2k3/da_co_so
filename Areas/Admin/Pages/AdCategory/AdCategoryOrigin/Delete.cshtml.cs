using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryOrigin
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
        public CategoryOrigin Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? originid)
        {
            if (originid == null)
            {
                return NotFound("No origin ID specified");
            }

            Category = await _context.CategoryOrigins.FindAsync(originid);

            if (Category == null)
            {
                return NotFound("Origin not found");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? originid)
        {
            if (originid == null)
            {
                return NotFound("No origin ID specified");
            }

            Category = await _context.CategoryOrigins.FindAsync(originid);

            if (Category == null)
            {
                return NotFound("Origin not found");
            }

            _context.CategoryOrigins.Remove(Category);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just deleted the origin: {Category.NameCategory}";

            return RedirectToPage("./Index");
        }
    }
}
