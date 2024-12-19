using System.Linq;
using Microsoft.AspNetCore.Mvc;
using do_an.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using do_an_ltweb.Models;
using Microsoft.EntityFrameworkCore;

namespace do_an.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavouriteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // [POST] /Favourite/Toggle
        [HttpPost]
        public IActionResult Toggle(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            // Kiểm tra xem sản phẩm đã tồn tại trong danh sách yêu thích chưa
            var existingFavorite = _context.Favourites.FirstOrDefault(f => f.IdUser == userId && f.IdProduct == productId);
            if (existingFavorite != null)
            {
                // Nếu đã tồn tại, xóa sản phẩm khỏi danh sách yêu thích
                _context.Favourites.Remove(existingFavorite);
                _context.SaveChanges();
                return Json(new { success = true, message = "Product removed from favorites", isFavorited = false });
            }

            // Nếu chưa tồn tại, thêm sản phẩm vào danh sách yêu thích
            var favorite = new Favourite
            {
                IdUser = userId,
                IdProduct = productId
            };
            _context.Favourites.Add(favorite);
            _context.SaveChanges();

            return Json(new { success = true, message = "Product added to favorites", isFavorited = true });
        }


        // [POST] /Favourite/Remove
        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            // Tìm sản phẩm yêu thích trong cơ sở dữ liệu
            var existingFavorite = _context.Favourites.FirstOrDefault(f => f.IdUser == userId && f.IdProduct == productId);
            if (existingFavorite != null)
            {
                // Xóa sản phẩm khỏi danh sách yêu thích
                _context.Favourites.Remove(existingFavorite);
                _context.SaveChanges();
                return Json(new { success = true, message = "Product removed from favorites" });
            }

            return Json(new { success = false, message = "Product not found in favorites" });
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách các sản phẩm yêu thích của người dùng
            var favoriteProducts = await _context.Favourites
                .Where(f => f.IdUser == userId)
                .Include(f => f.Product) // Load thông tin sản phẩm liên quan
                .Where(f => f.Product.Hide != 1) // Bỏ qua các sản phẩm bị ẩn
                .Select(f => f.Product) // Chỉ lấy sản phẩm từ bảng Favourites
                .ToListAsync();

            return View(favoriteProducts); // Trả về danh sách sản phẩm yêu thích
        }



    }
}
