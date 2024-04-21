using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using do_an_ltweb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryPrice
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Min")]
            //[Required(ErrorMessage = "Min or Max value is required.")]
            [Range(0, int.MaxValue, ErrorMessage = "Min value must be greater than or equal to 0.")]
            public int? Min { get; set; }

            [Display(Name = "Max")]
            //[Required(ErrorMessage = "Min or Max value is required.")]
            [Range(1, int.MaxValue, ErrorMessage = "Max value must be greater than 0.")]
            public int? Max { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra điều kiện: ít nhất một trong hai ô Min hoặc Max phải được nhập
            if (Input.Min == null && Input.Max == null)
            {
                ModelState.AddModelError(string.Empty, "At least one of Min or Max value is required.");
                return Page();
            }

            // Kiểm tra điều kiện: Max phải lớn hơn Min
            if (Input.Max.HasValue && Input.Min.HasValue && Input.Max <= Input.Min)
            {
                ModelState.AddModelError(string.Empty, "Max value must be greater than Min value.");
                return Page();
            }

            // Kiểm tra xem cả Min và Max đã tồn tại trong cơ sở dữ liệu và không giống với bất kỳ danh mục nào khác
            var existingCategory = await _context.CategoryPrices.FirstOrDefaultAsync(c =>
                (Input.Min == null || c.PriceMin == Input.Min) &&
                (Input.Max == null || c.PriceMax == Input.Max));

            if (existingCategory != null)
            {
                ModelState.AddModelError(string.Empty, "Category with the same Min and Max values already exists.");
                return Page();
            }

            // Tạo mới danh mục
            var newCategory = new CategoryPrice
            {
                PriceMin = Input.Min,
                PriceMax = Input.Max,
                Hide = 0 // Gán giá trị Hide thành 0
            };

            // Gán NameCategory dựa trên giá trị nhập vào
            if (Input.Min != null && Input.Max != null)
            {
                newCategory.NameCategory = $"{Input.Min:N0}đ - {Input.Max:N0}đ";
            }
            else if (Input.Min != null)
            {
                newCategory.NameCategory = $"Above {Input.Min:N0}đ";
            }
            else
            {
                newCategory.NameCategory = $"Under {Input.Max:N0}đ";
            }
            _context.CategoryPrices.Add(newCategory);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just created a new price category: {newCategory.NameCategory}";
            return RedirectToPage("./Index");
        }
    }
}
