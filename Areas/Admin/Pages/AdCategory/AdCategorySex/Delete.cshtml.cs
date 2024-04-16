using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdCategory.AdCategorySex
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
        public CategorySex Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? sexid)
        {
            if (sexid == null)
            {
                return NotFound("No sex ID specified");
            }

            Category = await _context.CategorySexes.FindAsync(sexid);

            if (Category == null)
            {
                return NotFound("Sex not found");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? sexid)
        {
            if (sexid == null)
            {
                return NotFound("No sex ID specified");
            }

            Category = await _context.CategorySexes.FindAsync(sexid);

            if (Category == null)
            {
                return NotFound("Sex not found");
            }

            _context.CategorySexes.Remove(Category);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just deleted the sex: {Category.NameCategory}";

            return RedirectToPage("./Index");
        }
    }
}
