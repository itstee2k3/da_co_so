
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace do_an.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryBrand _categoryBrand;

        public ProductController(IProductRepository productRepository, ICategoryBrand categoryBrand)
        {
            _productRepository = productRepository;
            _categoryBrand = categoryBrand;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var products = await _productRepository.GetAllAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                //products = products.Where(n => n.NameProduct.Contains(searchString)).ToList();
                products = products.Where(n => n.NameProduct.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1).ToList();
            }
            return View(products);
        }

        // Hiển thị thông tin chi tiết sản phẩm
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