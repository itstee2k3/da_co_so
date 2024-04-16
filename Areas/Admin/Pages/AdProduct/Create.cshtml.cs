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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace do_an_ltweb.Admin.AdProduct
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ApplicationDbContext _context;

        public CreateModel(IWebHostEnvironment webHostEnvironment, ApplicationDbContext applicationDbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = applicationDbContext;
        }

        [BindProperty]
        public InputModel Input { set; get; }

        public List<CategoryBrand> CategoryBrands { get; set; }
        public List<CategoryFrameColor> CategoryFrameColors { get; set; }
        public List<CategoryFrameStyle> CategoryFrameStyles { get; set; }
        public List<CategoryIrisColor> CategoryIrisColors { get; set; }
        public List<CategoryMaterial> CategoryMaterials { get; set; }
        public List<CategoryOrigin> CategoryOrigins { get; set; }
        public List<CategorySex> CategorySexes { get; set; }

        public class InputModel
        {
            [Display(Name = "Name of Product")]
            [Required(ErrorMessage = "Must enter {0}")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters long")]
            public string? Name { get; set; }

            [Display(Name = "Price")]
            [Required(ErrorMessage = "Must enter {0}")]
            [Range(1, int.MaxValue, ErrorMessage = "Price value must be greater than 0.")]
            public int? Price { get; set; }

            [Display(Name = "Nums")]
            [Required(ErrorMessage = "Must enter {0}")]
            [Range(1, int.MaxValue, ErrorMessage = "Nums value must be greater than 0.")]
            public int? Nums { get; set; }

            [Display(Name = "Description")]
            [StringLength(5000, MinimumLength = 20, ErrorMessage = "{0} must be between {2} and {1} characters long")]
            public string? Description { get; set; }

            [Display(Name = "Image")]
            public IFormFile? ImageUrl { get; set; }

            [Display(Name = "Size")]
            [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters long")]
            public string? Size { get; set; }

            [Display(Name = "Category Brand")]
            public int? CategoryBrandId { get; set; }

            [Display(Name = "Category Frame Color")]
            public int? CategoryFrameColorId { get; set; }

            [Display(Name = "Category Frame Style")]
            public int? CategoryFrameStyleId { get; set; }

            [Display(Name = "Category Iris Color")]
            public int? CategoryIrisColorId { get; set; }

            [Display(Name = "Category Material")]
            public int? CategoryMaterialId { get; set; }

            [Display(Name = "Category Origin")]
            public int? CategoryOriginId { get; set; }

            [Display(Name = "Category Sex")]
            public int? CategorySexId { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy danh sách các category brand từ database và gán cho CategoryBrands
            CategoryBrands = await _context.CategoryBrands.ToListAsync() ?? new List<CategoryBrand>();
            CategoryFrameColors = await _context.CategoryFrameColors.ToListAsync() ?? new List<CategoryFrameColor>();
            CategoryFrameStyles = await _context.CategoryFrameStyles.ToListAsync() ?? new List<CategoryFrameStyle>();
            CategoryIrisColors = await _context.CategoryIrisColors.ToListAsync() ?? new List<CategoryIrisColor>();
            CategoryMaterials = await _context.CategoryMaterials.ToListAsync() ?? new List<CategoryMaterial>();
            CategoryOrigins = await _context.CategoryOrigins.ToListAsync() ?? new List<CategoryOrigin>();
            CategorySexes = await _context.CategorySexes.ToListAsync() ?? new List<CategorySex>();

            // Tương tự, lấy danh sách cho các danh mục khác và gán cho các thuộc tính tương ứng

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra xem tên đã tồn tại trong cơ sở dữ liệu chưa
            if (_context.Products.Any(c => c.NameProduct == Input.Name))
            {
                ModelState.AddModelError(string.Empty, "Product name already exists.");
                return Page();
            }

            // Lưu ảnh và nhận đường dẫn
            string imageUrl = await SaveImage(Input.ImageUrl);


            // Tạo mới danh mục
            var newProduct = new Product
            {
                NameProduct = Input.Name,
                Hide = 0, // Gán giá trị Hide thành 0
                          // Gán các giá trị khác từ Input
                Price = Input.Price.GetValueOrDefault(), // Gán giá trị Price từ Input
                Nums = Input.Nums.GetValueOrDefault(), // Gán giá trị Nums từ Input
                Description = Input.Description,
                ImageUrl = imageUrl,
                Size = Input.Size,
                IdCategoryBrand = Input.CategoryBrandId,
                IdCategoryFrameColor = Input.CategoryFrameColorId,
                IdCategoryFrameStyle = Input.CategoryFrameStyleId,
                IdCategoryIrisColor = Input.CategoryIrisColorId,
                IdCategoryMaterial = Input.CategoryMaterialId,
                IdCategoryOrigin = Input.CategoryOriginId,
                IdCategorySex = Input.CategorySexId,


            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just created a new product: {Input.Name}";
            return RedirectToPage("./Index");
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            // Tạo đường dẫn thư mục lưu ảnh
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            // Tạo tên file duy nhất bằng cách kết hợp GUID với tên file gốc
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);

            // Tạo đường dẫn đầy đủ đến tệp ảnh
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Sao chép tệp ảnh vào thư mục lưu trữ
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                // Ví dụ: Ghi log, thông báo lỗi, vv.
                Console.WriteLine($"An error occurred while saving the image: {ex.Message}");
                return null;
            }

            // Trả về đường dẫn của tệp đã lưu
            return "/images/" + uniqueFileName; // Đường dẫn tương đối của ảnh
        }
    }
}
