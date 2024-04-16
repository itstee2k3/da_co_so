using do_an_ltweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace do_an.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy toàn bộ danh sách sản phẩm từ repository
            var allProducts = await _productRepository.GetAllAsync();

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
