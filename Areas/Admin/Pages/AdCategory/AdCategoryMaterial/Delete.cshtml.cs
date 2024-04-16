using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryMaterial
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
        public CategoryMaterial Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? materialid)
        {
            if (materialid == null)
            {
                return NotFound("No material ID specified");
            }

            Category = await _context.CategoryMaterials.FindAsync(materialid);

            if (Category == null)
            {
                return NotFound("Material not found");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? materialid)
        {
            if (materialid == null)
            {
                return NotFound("No material ID specified");
            }

            Category = await _context.CategoryMaterials.FindAsync(materialid);

            if (Category == null)
            {
                return NotFound("Material not found");
            }

            _context.CategoryMaterials.Remove(Category);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just deleted the material: {Category.NameCategory}";

            return RedirectToPage("./Index");
        }
    }
}
