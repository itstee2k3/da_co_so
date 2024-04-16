using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryFrameColor
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
        public CategoryFrameColor Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? framecolorid)
        {
            if (framecolorid == null)
            {
                return NotFound("No frame color ID specified");
            }

            Category = await _context.CategoryFrameColors.FindAsync(framecolorid);

            if (Category == null)
            {
                return NotFound("Frame color not found");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? framecolorid)
        {
            if (framecolorid == null)
            {
                return NotFound("No frame color ID specified");
            }

            Category = await _context.CategoryFrameColors.FindAsync(framecolorid);

            if (Category == null)
            {
                return NotFound("Frame color not found");
            }

            _context.CategoryFrameColors.Remove(Category);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just deleted the frame color: {Category.NameCategory}";

            return RedirectToPage("./Index");
        }
    }
}
