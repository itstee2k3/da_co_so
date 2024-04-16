
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

        public const int ITEMS_PER_PAGE = 15;

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

            // Tạo instance của PagingModel và đặt các thuộc tính
            var pagingModel = new PagingModel()
            {
                currentpage = currentPage,
                countpages = countPages
            };

            ViewBag.Products = products;

            // Lấy danh sách CategoryBrand và truyền vào view
            var categoryBrands = await _categoryBrand.GetAllAsync();
            ViewBag.CategoryBrands = categoryBrands;

            var categoryFrameStyles = await _categoryFrameStyle.GetAllAsync();
            ViewBag.CategoryFrameStyles = categoryFrameStyles;

            return View("Index", pagingModel);
        }


        public async Task<IActionResult> Index(string searchString)
        {
            // Lưu giá trị searchString vào session
            if (!String.IsNullOrEmpty(searchString))
            {
                // Lưu giá trị searchString vào session
                HttpContext.Session.SetString("searchString", searchString);
            }
            else
            {
                //HttpContext.Session.SetString("searchString", "");
                //HttpContext.Session.Remove("searchedProducts");
                HttpContext.Session.Remove("searchString");
            }

            // Lấy giá trị searchString từ session
            var searchStringFromSession = HttpContext.Session.GetString("searchString");

            // Kiểm tra nếu searchString không null hoặc rỗng
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
                else
                {
                    products = await _productRepository.GetAllAsync();
                }
            }

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

            // Tạo instance của PagingModel và đặt các thuộc tính
            var pagingModel = new PagingModel()
            {
                currentpage = currentPage,
                countpages = countPages
            };

            ViewBag.Products = products;

            // Lấy danh sách CategoryBrand và truyền vào view
            var categoryBrands = await _categoryBrand.GetAllAsync();
            ViewBag.CategoryBrands = categoryBrands;

            var categoryFrameStyles = await _categoryFrameStyle.GetAllAsync();
            ViewBag.CategoryFrameStyles = categoryFrameStyles;

            return View(pagingModel);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
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