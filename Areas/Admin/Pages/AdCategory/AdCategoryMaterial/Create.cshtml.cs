using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryMaterial
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public class InputModel
        {
            [Display(Name = "Name of CategoryMaterial")]
            [Required(ErrorMessage = "Must enter {0}")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters long")]
            public string Name { get; set; }
        }

        [BindProperty]
        public InputModel Input { set; get; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra xem tên đã tồn tại trong cơ sở dữ liệu chưa
            if (_context.CategoryMaterials.Any(c => c.NameCategory == Input.Name))
            {
                ModelState.AddModelError(string.Empty, "Category name already exists.");
                return Page();
            }

            // Tạo mới danh mục
            var newCategory = new CategoryMaterial
            {
                NameCategory = Input.Name,
                Hide = 0 // Gán giá trị Hide thành 0
            };
            _context.CategoryMaterials.Add(newCategory);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just created a new material category: {Input.Name}";
            return RedirectToPage("./Index");
        }
    }
}
