
using do_an_ltweb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace do_an.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryBrand _categoryBrand;
        private readonly ICategoryFrameStyle _categoryFrameStyle;

        public ProductController(ApplicationDbContext context, IProductRepository productRepository, ICategoryBrand categoryBrand, ICategoryFrameStyle categoryFrameStyle)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryBrand = categoryBrand;
            _categoryFrameStyle = categoryFrameStyle;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var products = await _productRepository.GetAllAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                //products = products.Where(n => n.NameProduct.Contains(searchString)).ToList();
                products = products.Where(n => n.NameProduct.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1).ToList();
            }
            ViewBag.Products = products;

            // Lấy danh sách CategoryBrand và truyền vào view
            var categoryBrands = await _categoryBrand.GetAllAsync();
            ViewBag.CategoryBrands = categoryBrands;

            var categoryFrameStyles = await _categoryFrameStyle.GetAllAsync();
            ViewBag.CategoryFrameStyles = categoryFrameStyles;

            return View();
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

        // Key lưu chuỗi json của Cart
        //public const string CARTKEY = "cart";

        ///// Thêm sản phẩm vào cart
        ////[HttpGet("/products/detail/{id}")]
        //public IActionResult AddToCart( int id, [FromQuery] int quantity)
        //{
        //    quantity = 1;
        //    var product = _context.Products
        //        .Where(p => p.IdProduct == id)
        //        .FirstOrDefault();
        //    if (product == null)
        //        return NotFound("Không có sản phẩm");

        //    // Xử lý đưa vào Cart ...
        //    var cart = GetCartItems();
        //    var cartitem = cart.Find(p => p.Product.IdProduct == id);
        //    if (cartitem != null)
        //    {
        //        // Đã tồn tại, tăng thêm 1
        //        cartitem.Quantity += quantity;
        //    }
        //    else
        //    {
        //        //  Thêm mới
        //        cart.Add(new CartItem() { Quantity = quantity, Product = product });
        //    }

        //    // Lưu cart vào Session
        //    SaveCartSession(cart);
        //    // Chuyển đến trang hiện thị Cart
        //    return RedirectToAction(nameof(Cart));
        //}

        ///// xóa item trong cart
        //public IActionResult RemoveCart([FromRoute] int productId)
        //{

        //    // Xử lý xóa một mục của Cart ...
        //    return RedirectToAction(nameof(Cart));
        //}

        ///// Cập nhật
        //[HttpPost]
        //public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
        //{
        //    // Cập nhật Cart thay đổi số lượng quantity ...

        //    return RedirectToAction(nameof(Cart));
        //}


        ////// Hiện thị giỏ hàng
        ////[Route("/cart", Name = "cart")]
        //public IActionResult Cart()
        //{
        //    return RedirectToAction("Index","Cart");
        //}

        //[Route("/checkout")]
        //public IActionResult CheckOut()
        //{
        //    // Xử lý khi đặt hàng
        //    return View();
        //}

        //// Lấy cart từ Session (danh sách CartItem)
        //List<CartItem> GetCartItems()
        //{

        //    var session = HttpContext.Session;
        //    string jsoncart = session.GetString(CARTKEY);
        //    if (jsoncart != null)
        //    {
        //        return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
        //    }
        //    return new List<CartItem>();
        //}

        //// Xóa cart khỏi session
        //void ClearCart()
        //{
        //    var session = HttpContext.Session;
        //    session.Remove(CARTKEY);
        //}

        //// Lưu Cart (Danh sách CartItem) vào session
        //void SaveCartSession(List<CartItem> ls)
        //{
        //    var session = HttpContext.Session;
        //    string jsoncart = JsonConvert.SerializeObject(ls);
        //    session.SetString(CARTKEY, jsoncart);
        //}
    }
}