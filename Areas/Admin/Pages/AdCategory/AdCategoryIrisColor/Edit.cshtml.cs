using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using do_an.Models;
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryIrisColor
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public CategoryIrisColor Category { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Must enter {0}")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} must be {2} to {1} characters long")]
            public string NameCategory { get; set; }

            public int? Hide { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int iriscolorid)
        {
            if (iriscolorid == 0)
            {
                return NotFound();
            }

            Category = await _context.CategoryIrisColors.FirstOrDefaultAsync(m => m.IdCategory == iriscolorid);

            if (Category == null)
            {
                return NotFound();
            }

            // Gán giá trị từ CategoryBrand vào Input để đổ dữ liệu vào form
            Input = new InputModel
            {
                NameCategory = Category.NameCategory,
                Hide = Category.Hide
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int iriscolorid)
        {
            if (iriscolorid == 0) return NotFound("No iris color found");

            Category = await _context.CategoryIrisColors.FindAsync(iriscolorid);
            if (Category == null) return NotFound("No iris color found");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra xem tên đã tồn tại trong cơ sở dữ liệu chưa (ngoại trừ danh mục đang được cập nhật)
            if (_context.CategoryIrisColors.Any(c => c.NameCategory == Input.NameCategory && c.IdCategory != iriscolorid))
            {
                ModelState.AddModelError(string.Empty, "Category name already exists.");
                return Page();
            }

            Category.NameCategory = Input.NameCategory;
            Category.Hide = Input.Hide;

            _context.CategoryIrisColors.Update(Category);

            try
            {
                await _context.SaveChangesAsync();
                StatusMessage = $"You have just update the iris color: {Input.NameCategory}";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

    }
}
