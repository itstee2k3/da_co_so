using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryIrisColor
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
        public CategoryIrisColor Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? iriscolorid)
        {
            if (iriscolorid == null)
            {
                return NotFound("No iris color ID specified");
            }

            Category = await _context.CategoryIrisColors.FindAsync(iriscolorid);

            if (Category == null)
            {
                return NotFound("Iris color not found");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? iriscolorid)
        {
            if (iriscolorid == null)
            {
                return NotFound("No iris color ID specified");
            }

            Category = await _context.CategoryIrisColors.FindAsync(iriscolorid);

            if (Category == null)
            {
                return NotFound("Iris color not found");
            }

            _context.CategoryIrisColors.Remove(Category);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just deleted the iris color: {Category.NameCategory}";

            return RedirectToPage("./Index");
        }
    }
}
