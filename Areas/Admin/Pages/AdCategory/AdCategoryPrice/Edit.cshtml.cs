using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using do_an_ltweb.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace do_an_ltweb.Admin.AdCategory.AdCategoryPrice
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Min")]
            [Range(0, int.MaxValue, ErrorMessage = "Min value must be greater than or equal to 0.")]
            public int? Min { get; set; }

            [Display(Name = "Max")]
            [Range(1, int.MaxValue, ErrorMessage = "Max value must be greater than 0.")]
            public int? Max { get; set; }

            public int? Hide { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int priceid)
        {
            var categoryPrice = await _context.CategoryPrices.FindAsync(priceid);

            if (categoryPrice == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Min = categoryPrice.PriceMin,
                Max = categoryPrice.PriceMax,
                Hide = categoryPrice.Hide
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int priceid)
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

            // Kiểm tra xem có CategoryPrice nào khác có cùng Min và Max không (ngoại trừ CategoryPrice hiện tại đang chỉnh sửa)
            var existingCategory = await _context.CategoryPrices.FirstOrDefaultAsync(c =>
                (Input.Min == null || c.PriceMin == Input.Min) &&
                (Input.Max == null || c.PriceMax == Input.Max) &&
                c.IdCategory != priceid);

            if (existingCategory != null)
            {
                ModelState.AddModelError(string.Empty, "Category with the same Min and Max values already exists.");
                return Page();
            }

            // Lấy thông tin của CategoryPrice cần chỉnh sửa
            var categoryPriceToUpdate = await _context.CategoryPrices.FindAsync(priceid);

            if (categoryPriceToUpdate == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin của CategoryPrice
            categoryPriceToUpdate.PriceMin = Input.Min;
            categoryPriceToUpdate.PriceMax = Input.Max;
            categoryPriceToUpdate.Hide = Input.Hide;

            // Gán lại tên cho CategoryPrice dựa trên Min và Max
            if (Input.Min != null && Input.Max != null)
            {
                categoryPriceToUpdate.NameCategory = $"{Input.Min:N0}đ - {Input.Max:N0}đ";
            }
            else if (Input.Min != null)
            {
                categoryPriceToUpdate.NameCategory = $"Over {Input.Min:N0}đ";
            }
            else
            {
                categoryPriceToUpdate.NameCategory = $"Under {Input.Max:N0}đ";
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just updated the price category: {categoryPriceToUpdate.NameCategory}";
            return RedirectToPage("./Index");
        }
    }
}
