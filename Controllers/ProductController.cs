
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using XTL.Helpers; // Đây là namespace chứa lớp PagingModel

namespace do_an.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryBrand _categoryBrand;
        private readonly ICategoryFrameStyle _categoryFrameStyle;

        IEnumerable<Product> products = null;

        public const int ITEMS_PER_PAGE = 9;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }

        public int countPages { get; set; }

        public int totalProducts { get; set; }

        public ProductController(ApplicationDbContext context, IProductRepository productRepository, ICategoryBrand categoryBrand, ICategoryFrameStyle categoryFrameStyle)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryBrand = categoryBrand;
            _categoryFrameStyle = categoryFrameStyle;
        }

        public async Task<IActionResult> AllProducts()
        {
            // Khai báo và khởi tạo biến currentPage
            int currentPage = 1;

            products = await _productRepository.GetAllAsync();

            // Xóa sessionStorage bằng JavaScript
            ViewBag.ClearSessionStorageScript = "sessionStorage.clear();";

            // Xóa tất cả các session
            HttpContext.Session.Clear();

            // Kiểm tra xem có sản phẩm nào không
            if (products.Any())
            {
                // Tính toán số lượng sản phẩm
                totalProducts = products.Count();
                countPages = (int)Math.Ceiling((double)totalProducts / ITEMS_PER_PAGE);

                // Đảm bảo trang hiện tại không vượt quá số trang có sẵn
                currentPage = Math.Clamp(currentPage, 1, countPages);
            }

            // Lọc danh sách sản phẩm cho trang hiện tại
            products = products.Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();

            // Tạo instance của PagingModel và đặt các thuộc tính
            var pagingModel = new PagingModel()
            {
                currentpage = currentPage,
                countpages = countPages
            };

            ViewBag.Products = products;

            ViewBag.TotalProducts = totalProducts; // Thêm giá trị TotalProducts vào ViewBag

            // Lấy danh sách CategoryBrand và truyền vào view
            var categoryBrands = await _categoryBrand.GetAllAsync();
            ViewBag.CategoryBrands = categoryBrands.OrderBy(brand => brand.NameCategory).ToList();  // Sắp xếp ngay khi lấy;

            var categoryFrameStyles = await _categoryFrameStyle.GetAllAsync();
            ViewBag.CategoryFrameStyles = categoryFrameStyles.OrderBy(fss => fss.NameCategory).ToList();

            return View("Index", pagingModel);
        }

        [HttpGet]
        public string GetSearchString()
        {
            var searchString = HttpContext.Session.GetString("searchString");
            return searchString ?? "";
        }

        // Phương thức để lấy tên của danh mục từ ID của danh mục thương hiệu
        private async Task<string> GetNameCategoryBrandById(int? categoryId)
        {
            var categoryBrand = await _context.CategoryBrands.FindAsync(categoryId);
            return categoryBrand?.NameCategory;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, string[] selectedCategories, int p = 1)
        {
            // Khai báo và khởi tạo biến currentPage
            int currentPage = p;

            // Lấy giá trị searchString từ session
            var searchStringFromSession = HttpContext.Session.GetString("searchString");
            // Lấy giá trị sortOrder từ session
            var sortOrderFromSession = HttpContext.Session.GetString("sortOrder");
            // Lấy giá trị selectedCategories từ session
            var selectedCategoriesFromSession = HttpContext.Session.GetString("selectedCategories");

            // Lấy danh sách sản phẩm từ repository
            var products = await _productRepository.GetAllAsync();

            // Lưu giá trị searchString vào session nếu có
            if (!String.IsNullOrEmpty(searchString))
            {
                HttpContext.Session.SetString("searchString", searchString);
            }
            else
            {
                // Kiểm tra xem searchString đã được lưu trong session chưa
                if (!String.IsNullOrEmpty(searchStringFromSession))
                {
                    // Sử dụng giá trị từ session nếu nó tồn tại
                    searchString = searchStringFromSession;
                }
            }

            // Lưu giá trị sortOrder vào session nếu có
            if (!String.IsNullOrEmpty(sortOrder) && sortOrder != "Default")
            {
                HttpContext.Session.SetString("sortOrder", sortOrder);
            }
            else
            {
                // Kiểm tra xem sortOrder đã được lưu trong session chưa
                if (!String.IsNullOrEmpty(sortOrderFromSession) && sortOrder != "Default")
                {
                    // Sử dụng giá trị từ session nếu nó tồn tại
                    sortOrder = sortOrderFromSession;
                }
            }

            // Lưu giá trị selectedCategories vào session nếu có
            if (selectedCategories != null && selectedCategories.Length > 0)
            {
                HttpContext.Session.SetString("selectedCategories", JsonConvert.SerializeObject(selectedCategories));
            }
            else
            {
                // Kiểm tra xem selectedCategories đã được lưu trong session chưa
                if (!string.IsNullOrEmpty(selectedCategoriesFromSession) && selectedCategories.Length > 0)
                {
                    // Sử dụng giá trị từ session nếu nó tồn tại
                    selectedCategories = JsonConvert.DeserializeObject<string[]>(selectedCategoriesFromSession);
                }
            }

            // Kiểm tra và khởi tạo biến currentPage
            currentPage = currentPage > 0 ? currentPage : 1;

            // Tìm kiếm sản phẩm nếu có searchString
            if (!String.IsNullOrEmpty(searchString))
            {
                // Tìm kiếm sản phẩm và lưu vào session
                var searchedProducts = products.Where(n => n.NameProduct.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1).ToList();
                HttpContext.Session.SetString("searchedProducts", JsonConvert.SerializeObject(searchedProducts));

                // Reset currentPage về 1 khi có tìm kiếm
                currentPage = p;
                HttpContext.Session.SetInt32("currentPage", currentPage);

                products = searchedProducts;
            }
            else
            {
                // Lấy danh sách sản phẩm từ session (nếu có)
                var productsJson = HttpContext.Session.GetString("searchedProducts");
                if (!String.IsNullOrEmpty(productsJson))
                {
                    products = JsonConvert.DeserializeObject<List<Product>>(productsJson);
                }
            }

            // Kiểm tra và áp dụng sắp xếp nếu có sortOrder
            switch (sortOrder)
            {
                case "PriceAsc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "PriceDesc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "Default":
                default:
                    HttpContext.Session.Remove("sortOrder");
                    break;
            }

            // Lọc sản phẩm theo name danh mục đã chọn
            if (selectedCategories != null && selectedCategories.Length > 0)
            {

                // Chuyển đổi các name danh mục từ chuỗi sang một mảng 
                var categoryNames = selectedCategories
                    .SelectMany(ids => ids.Split(',')) // Tách các ID nếu có nhiều ID được truyền qua
                    .ToList();

                // Lấy category name từ Id và kiểm tra xem nó có tồn tại trong mảng selectedCategories không
                products = products.Where(p => p.IdCategoryBrand != null &&
                    categoryNames.Any(name => name == GetNameCategoryBrandById(p.IdCategoryBrand).Result)).ToList();

            }

            // Tính toán số lượng sản phẩm và phân trang
            var totalProducts = products.Count();
            var countPages = (int)Math.Ceiling((double)totalProducts / ITEMS_PER_PAGE);

            // Đảm bảo countPages không nhỏ hơn 0
            countPages = Math.Max(countPages, 0);

            // Đảm bảo trang hiện tại không vượt quá số trang có sẵn
            if (countPages > 0)
            {
                currentPage = Math.Clamp(currentPage, 1, countPages);
            }
            else
            {
                currentPage = 1; // Trang hiện tại luôn là 1 nếu không có sản phẩm nào
            }

            // Lọc danh sách sản phẩm cho trang hiện tại
            products = products.Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();

            // Tạo instance của PagingModel và đặt các thuộc tính
            var pagingModel = new PagingModel()
            {
                currentpage = currentPage,
                countpages = countPages
            };

            ViewBag.SortOrderFromSession = sortOrder;
            ViewBag.Products = products;
            ViewBag.TotalProducts = totalProducts;

            // Lấy danh sách CategoryBrand và CategoryFrameStyles và truyền vào view
            var categoryBrands = await _categoryBrand.GetAllAsync();
            ViewBag.CategoryBrands = categoryBrands.OrderBy(brand => brand.NameCategory).ToList();

            var categoryFrameStyles = await _categoryFrameStyle.GetAllAsync();
            ViewBag.CategoryFrameStyles = categoryFrameStyles.OrderBy(fss => fss.NameCategory).ToList();

            return View(pagingModel);
        }

        public async Task<IActionResult> Detail(int id)
        {
            // Lấy sản phẩm từ repository
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Kiểm tra xem sản phẩm có danh mục thương hiệu không
            if (product.IdCategoryBrand != null)
            {
                // Lấy thông tin về danh mục thương hiệu từ cơ sở dữ liệu
                var categoryBrand = await _context.CategoryBrands.FindAsync(product.IdCategoryBrand);

                // Kiểm tra xem danh mục thương hiệu có tồn tại không
                if (categoryBrand != null)
                {
                    // Đặt thông tin về danh mục thương hiệu vào sản phẩm
                    product.CategoryBrand = categoryBrand;
                }
            }

            // Kiểm tra xem sản phẩm có danh mục không
            if (product.IdCategoryFrameColor != null)
            {
                // Lấy thông tin về danh mục từ cơ sở dữ liệu
                var categoryFrameColor = await _context.CategoryFrameColors.FindAsync(product.IdCategoryFrameColor);

                // Kiểm tra xem danh mục thương hiệu có tồn tại không
                if (categoryFrameColor != null)
                {
                    // Đặt thông tin về danh mục thương hiệu vào sản phẩm
                    product.CategoryFrameColor = categoryFrameColor;
                }
            }

            // Kiểm tra xem sản phẩm có danh mục xuất xứ không
            if (product.IdCategoryOrigin != null)
            {
                // Lấy thông tin về danh mục từ cơ sở dữ liệu
                var categoryOrigin = await _context.CategoryOrigins.FindAsync(product.IdCategoryOrigin);

                // Kiểm tra xem danh mục có tồn tại không
                if (categoryOrigin != null)
                {
                    // Đặt thông tin về danh mục vào sản phẩm
                    product.CategoryOrigin = categoryOrigin;
                }
            }

            // Kiểm tra xem sản phẩm có danh mục không
            if (product.IdCategoryIrisColor != null)
            {
                // Lấy thông tin về danh mục thương hiệu từ cơ sở dữ liệu
                var categoryIrisColor = await _context.CategoryIrisColors.FindAsync(product.IdCategoryIrisColor);

                // Kiểm tra xem danh mục thương hiệu có tồn tại không
                if (categoryIrisColor != null)
                {
                    // Đặt thông tin về danh mục thương hiệu vào sản phẩm
                    product.CategoryIrisColor = categoryIrisColor;
                }
            }

            // Trả về view và truyền sản phẩm cho view
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}