
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            products = await _productRepository.GetAllAsync();

            // Xóa tất cả các session
            HttpContext.Session.Clear();

            //ViewBag.SearchString = ""; // or ViewBag.SearchString = "";

            // Tính toán số lượng sản phẩm
            totalProducts = products.Count(); // Số lượng sản phẩm trên trang hiện tại nếu có tìm kiếm
            countPages = (int)Math.Ceiling((double)totalProducts / ITEMS_PER_PAGE);

            // Đảm bảo trang hiện tại không vượt quá số trang có sẵn
            if (countPages > 0)
            {
                currentPage = Math.Clamp(currentPage, 1, countPages);
            }
            else
            {
                currentPage = 1;
            }

            // Lọc danh sách sản phẩm cho trang hiện tại
            products = products.Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();

            //// Cập nhật giá trị totalProducts sau khi lọc danh sách sản phẩm
            //totalProducts = products.Count();

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
            ViewBag.CategoryBrands = categoryBrands;

            var categoryFrameStyles = await _categoryFrameStyle.GetAllAsync();
            ViewBag.CategoryFrameStyles = categoryFrameStyles;

            return View("Index", pagingModel);
        }


        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // Lưu giá trị sortOrder vào session nếu có
            if (!String.IsNullOrEmpty(sortOrder))
            {
                HttpContext.Session.SetString("sortOrder", sortOrder);
            }
            //else
            //{
            //    // Nếu không có sortOrder, loại bỏ nó khỏi session
            //    HttpContext.Session.Remove("sortOrder");
            //}

            // Lấy giá trị sortOrder từ session
            var sortOrderFromSession = HttpContext.Session.GetString("sortOrder");

            // Lấy danh sách sản phẩm từ repository
            var products = await _productRepository.GetAllAsync();

            // Lưu giá trị searchString vào session nếu có
            if (!String.IsNullOrEmpty(searchString))
            {
                HttpContext.Session.SetString("searchString", searchString);
            }
            else
            {
                HttpContext.Session.Remove("searchString");
            }

            // Lấy giá trị searchString từ session
            var searchStringFromSession = HttpContext.Session.GetString("searchString");

            // Lưu giá trị currentPage vào session nếu có
            if (currentPage > 0)
            {
                HttpContext.Session.SetInt32("currentPage", currentPage);
            }

            // Lấy giá trị currentPage từ session
            var currentPageFromSession = HttpContext.Session.GetInt32("currentPage");

            // Sử dụng giá trị currentPage từ session hoặc mặc định là 1 nếu không có
            currentPage = currentPageFromSession ?? 1;

            // Tìm kiếm sản phẩm nếu có searchString
            if (!String.IsNullOrEmpty(searchStringFromSession))
            {
                // Tìm kiếm sản phẩm và lưu vào session
                var searchedProducts = await _productRepository.GetAllAsync();
                searchedProducts = searchedProducts.Where(n => n.NameProduct.IndexOf(searchStringFromSession, StringComparison.OrdinalIgnoreCase) != -1).ToList();
                HttpContext.Session.SetString("searchedProducts", JsonConvert.SerializeObject(searchedProducts));

                // Reset currentPage về 1 khi có tìm kiếm
                currentPage = 1;
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
            if (!String.IsNullOrEmpty(sortOrderFromSession))
            {
                switch (sortOrderFromSession)
                {
                    case "PriceAsc":
                        products = products.OrderBy(p => p.Price).ToList();
                        break;
                    case "PriceDesc":
                        products = products.OrderByDescending(p => p.Price).ToList();
                        break;
                    case "Default":
                        HttpContext.Session.Remove("sortOrder");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // Lấy danh sách sản phẩm từ session (nếu có)
                var productsJson = HttpContext.Session.GetString("sortOrder");
                if (!String.IsNullOrEmpty(productsJson))
                {
                    products = JsonConvert.DeserializeObject<List<Product>>(productsJson);
                }
            }

            // Tính toán số lượng sản phẩm và phân trang
            totalProducts = products.Count();
            countPages = (int)Math.Ceiling((double)totalProducts / ITEMS_PER_PAGE);

            // Đảm bảo trang hiện tại không vượt quá số trang có sẵn
            if (countPages > 0)
            {
                currentPage = Math.Clamp(currentPage, 1, countPages);
            }
            else
            {
                currentPage = 1; // Nếu countPages <= 0, đặt currentPage về 1
            }

            // Lọc danh sách sản phẩm cho trang hiện tại
            products = products.Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();

            // Tạo instance của PagingModel và đặt các thuộc tính
            var pagingModel = new PagingModel()
            {
                currentpage = currentPage,
                countpages = countPages
            };

            ViewBag.SortOrderFromSession = sortOrderFromSession;

            ViewBag.Products = products;

            // Lấy danh sách CategoryBrand và CategoryFrameStyles và truyền vào view
            var categoryBrands = await _categoryBrand.GetAllAsync();
            ViewBag.CategoryBrands = categoryBrands;

            var categoryFrameStyles = await _categoryFrameStyle.GetAllAsync();
            ViewBag.CategoryFrameStyles = categoryFrameStyles;

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