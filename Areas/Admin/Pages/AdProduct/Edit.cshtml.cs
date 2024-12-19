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
using System.Text;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;

namespace do_an_ltweb.Admin.AdProduct
{
    [Authorize(Roles = "admin")]
    public class EidtModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ApplicationDbContext _context;

        public EidtModel(IWebHostEnvironment webHostEnvironment, ApplicationDbContext applicationDbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = applicationDbContext;

            Input = new InputModel();
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

            [Display(Name = "BRImageUrl")]
            public IFormFile? BRImageUrl { get; set; }

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

            [Display(Name = "Hide")]
            public int? Hide { get; set; }

            // Đường dẫn hình ảnh đã có sẵn
            public string? ExistingImageUrl { get; set; }
            public string? ExistingBRImageUrl { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int productid)
        {
            var product = await _context.Products.FindAsync(productid);

            if (product == null)
            {
                return NotFound();
            }

            // Lấy danh sách các category brand từ database và gán cho CategoryBrands
            CategoryBrands = await _context.CategoryBrands.ToListAsync();
            CategoryFrameColors = await _context.CategoryFrameColors.ToListAsync();
            CategoryFrameStyles = await _context.CategoryFrameStyles.ToListAsync();
            CategoryIrisColors = await _context.CategoryIrisColors.ToListAsync();
            CategoryMaterials = await _context.CategoryMaterials.ToListAsync();
            CategoryOrigins = await _context.CategoryOrigins.ToListAsync();
            CategorySexes = await _context.CategorySexes.ToListAsync();

            // Truyền giá trị của sản phẩm vào InputModel
            Input = new InputModel
            {
                Name = product.NameProduct,
                Price = (int?)product.Price,
                Nums = product.Nums,
                Description = product.Description,
                ExistingImageUrl = product.ImageUrl,
                ExistingBRImageUrl = product.BRImageUrl,
                Size = product.Size,
                CategoryBrandId = product.IdCategoryBrand,
                CategoryFrameColorId = product.IdCategoryFrameColor,
                CategoryFrameStyleId = product.IdCategoryFrameStyle,
                CategoryIrisColorId = product.IdCategoryIrisColor,
                CategoryMaterialId = product.IdCategoryMaterial,
                CategoryOriginId = product.IdCategoryOrigin,
                CategorySexId = product.IdCategorySex,
                Hide = product.Hide
            };

            // Kiểm tra xem ImageUrl có null hay không trước khi chuyển đổi
            if (Input.ImageUrl != null)
            {
                ViewData["PreviewImageUrl"] = Url.Content("~/images/" + Input.ImageUrl.FileName);
            }
            else
            {
                ViewData["PreviewImageUrl"] = null;
            }

            // Kiểm tra xem BRImageUrl có null hay không trước khi chuyển đổi
            if (Input.BRImageUrl != null)
            {
                ViewData["PreviewBRImageUrl"] = Url.Content("~/images/brimage" + Input.BRImageUrl.FileName);
            }
            else
            {
                ViewData["PreviewBRImageUrl"] = null;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int productid)
        {
            var productToUpdate = await _context.Products.FindAsync(productid);

            if (productToUpdate == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");

                CategoryBrands = await _context.CategoryBrands.ToListAsync();
                CategoryFrameColors = await _context.CategoryFrameColors.ToListAsync();
                CategoryFrameStyles = await _context.CategoryFrameStyles.ToListAsync();
                CategoryIrisColors = await _context.CategoryIrisColors.ToListAsync();
                CategoryMaterials = await _context.CategoryMaterials.ToListAsync();
                CategoryOrigins = await _context.CategoryOrigins.ToListAsync();
                CategorySexes = await _context.CategorySexes.ToListAsync();

                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"ModelState Error for {key}: {error.ErrorMessage}");
                    }
                }
                    // Log lỗi ModelState nếu có
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }

                return Page();
            }

            // Kiểm tra xem tên đã tồn tại trong cơ sở dữ liệu chưa
            if (_context.Products.Any(c => c.NameProduct == Input.Name && c.IdProduct != productid))
            {
                ModelState.AddModelError(string.Empty, "Product name already exists.");

                CategoryBrands = await _context.CategoryBrands.ToListAsync();
                CategoryFrameColors = await _context.CategoryFrameColors.ToListAsync();
                CategoryFrameStyles = await _context.CategoryFrameStyles.ToListAsync();
                CategoryIrisColors = await _context.CategoryIrisColors.ToListAsync();
                CategoryMaterials = await _context.CategoryMaterials.ToListAsync();
                CategoryOrigins = await _context.CategoryOrigins.ToListAsync();
                CategorySexes = await _context.CategorySexes.ToListAsync();

                return Page();
            }

            // Cập nhật thông tin sản phẩm
            productToUpdate.NameProduct = Input.Name;
            productToUpdate.Hide = Input.Hide.GetValueOrDefault(); // Gán giá trị Hide từ Input
            productToUpdate.Price = Input.Price.GetValueOrDefault();
            productToUpdate.Nums = Input.Nums.GetValueOrDefault();
            productToUpdate.Description = Input.Description;
            //productToUpdate.ImageUrl = imageUrl;

            // Kiểm tra xem có ảnh mới được tải lên không

            // Xử lý ảnh thường
            if (Input.ImageUrl != null)
            {
                // Xoá ảnh cũ
                if (!string.IsNullOrEmpty(productToUpdate.ImageUrl))
                {
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToUpdate.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                        catch (Exception ex)
                        {
                            // Ghi log lỗi nếu cần
                            Console.WriteLine($"Error deleting old image: {ex.Message}");
                        }
                    }
                }
                string imageUrl = await SaveImage(Input.ImageUrl);
                productToUpdate.ImageUrl = imageUrl; // Gán trực tiếp nếu có ảnh mới
            }
            else if (!string.IsNullOrEmpty(Input.ExistingImageUrl))
            {
                productToUpdate.ImageUrl = Input.ExistingImageUrl; // Dùng ảnh cũ nếu không có ảnh mới

            }
            //else
            //{
            //    //ModelState.AddModelError("ImageUrl", "Image is required.");

            //    productToUpdate.ImageUrl = null;
            //}

            if (Input.BRImageUrl != null)
            {
                // Xoá ảnh cũ
                if (!string.IsNullOrEmpty(productToUpdate.BRImageUrl))
                {
                    string oldBRImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToUpdate.BRImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldBRImagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldBRImagePath);
                        }
                        catch (Exception ex)
                        {
                            // Ghi log lỗi nếu cần
                            Console.WriteLine($"Error deleting old BR image: {ex.Message}");
                        }
                    }
                }
                string brImageUrl = await SaveBRImage(Input.BRImageUrl);
                productToUpdate.BRImageUrl = brImageUrl;
            }
            else if (string.IsNullOrEmpty(Input.ExistingBRImageUrl))
            {
                productToUpdate.BRImageUrl = Input.ExistingBRImageUrl;
            }
            //else
            //{
            //    //ModelState.AddModelError("BRImageUrl", "Brand Image is required.");

            //    productToUpdate.BRImageUrl = null;
            //}
            //TempData["ErrorMessage"] = "An error occurred while updating the product.";

            productToUpdate.Size = Input.Size;
            productToUpdate.IdCategoryBrand = Input.CategoryBrandId;
            productToUpdate.IdCategoryFrameColor = Input.CategoryFrameColorId;
            productToUpdate.IdCategoryFrameStyle = Input.CategoryFrameStyleId;
            productToUpdate.IdCategoryIrisColor = Input.CategoryIrisColorId;
            productToUpdate.IdCategoryMaterial = Input.CategoryMaterialId;
            productToUpdate.IdCategoryOrigin = Input.CategoryOriginId;
            productToUpdate.IdCategorySex = Input.CategorySexId;

            _context.Products.Update(productToUpdate);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"You have just updated the product: {Input.Name}";
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
            using (var imageStream = imageFile.OpenReadStream())
            {
                // Đọc ảnh từ stream
                var image = SixLabors.ImageSharp.Image.Load(imageStream);

                // Tính toán kích thước để cắt thành hình vuông
                int size = Math.Min(image.Width, image.Height);

                // Cắt và thay đổi kích thước ảnh
                var resizedImage = image.Clone(x => x.Crop(new Rectangle((image.Width - size) / 2, (image.Height - size) / 2, size, size))
                                                     .Resize(size, size));

                // Lưu ảnh đã chỉnh sửa
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await resizedImage.SaveAsJpegAsync(fileStream);
                }
            }

            // Trả về đường dẫn của tệp đã lưu
            return "/images/" + uniqueFileName; // Đường dẫn tương đối của ảnh
        }

        private async Task<string> SaveBRImage(IFormFile brImageFile)
        {
            if (brImageFile == null || brImageFile.Length == 0)
            {
                return null;
            }

            // Đảm bảo thư mục lưu ảnh tồn tại
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "brimage");
            //Directory.CreateDirectory(uploadsFolder);

            // Tạo tên file duy nhất
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(brImageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Xử lý ảnh với ImageSharp
            try
            {
                using (var imageStream = brImageFile.OpenReadStream())
                {
                    // Đọc ảnh từ stream
                    var image = SixLabors.ImageSharp.Image.Load(imageStream);

                    // Tính toán và cắt thành hình vuông
                    int size = Math.Min(image.Width, image.Height);
                    var resizedImage = image.Clone(x => x.Crop(new Rectangle((image.Width - size) / 2, (image.Height - size) / 2, size, size))
                                                     .Resize(size, size));

                    // Lưu ảnh đã chỉnh sửa
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await resizedImage.SaveAsJpegAsync(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the BR image: {ex.Message}");
                return null;
            }

            // Trả về đường dẫn tương đối của file
            return "/images/brimage/" + uniqueFileName;
        }

    }
}
