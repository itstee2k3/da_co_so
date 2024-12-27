using do_an_ltweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace do_an.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, IProductRepository productRepository)
        {
            _context = context;
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy toàn bộ danh sách sản phẩm từ repository
            var allProducts = await _productRepository.GetAllAsync();

            // Lấy userId từ thông tin xác thực
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Lấy danh sách sản phẩm yêu thích của người dùng
            var favoriteProductIds = new List<int>();
            if (!string.IsNullOrEmpty(userId))
            {
                favoriteProductIds = await _context.Favourites
                    .Where(f => f.IdUser == userId)
                    .Select(f => f.IdProduct)
                    .ToListAsync();
            }

            ViewBag.FavoriteProducts = favoriteProductIds;

            // Lấy 8 sản phẩm ngẫu nhiên từ danh sách sản phẩm
            var randomProducts = allProducts.OrderBy(p => Guid.NewGuid()).Take(8).ToList();

            // Gán danh sách sản phẩm ngẫu nhiên cho ViewBag.Products
            ViewBag.Products = randomProducts;

            // Trả về view
            return View();
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
