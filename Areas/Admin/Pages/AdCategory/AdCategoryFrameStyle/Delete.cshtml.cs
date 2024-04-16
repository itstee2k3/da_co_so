using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryFrameStyle
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
        public CategoryFrameStyle Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? framestyleid)
        {
            if (framestyleid == null)
            {
                return NotFound("No fram style ID specified");
            }

            Category = await _context.CategoryFrameStyles.FindAsync(framestyleid);

            if (Category == null)
            {
                return NotFound("Frame style not found");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? framestyleid)
        {
            if (framestyleid == null)
            {
                return NotFound("No frame style ID specified");
            }

            Category = await _context.CategoryFrameStyles.FindAsync(framestyleid);

            if (Category == null)
            {
                return NotFound("Frame style not found");
            }

            _context.CategoryFrameStyles.Remove(Category);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just deleted the frame styel: {Category.NameCategory}";

            return RedirectToPage("./Index");
        }
    }
}
