using Microsoft.EntityFrameworkCore;

namespace do_an_ltweb.Models
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdBrand(int categoryId)
        {
            return await _context.Products.Where(p => p.IdCategoryBrand == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdFrameColor(int categoryId)
        {
            return await _context.Products.Where(p => p.IdCategoryFrameColor == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdFrameStyle(int categoryId)
        {
            return await _context.Products.Where(p => p.IdCategoryFrameStyle == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdIrisColor(int categoryId)
        {
            return await _context.Products.Where(p => p.IdCategoryIrisColor == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdMaterial(int categoryId)
        {
            return await _context.Products.Where(p => p.IdCategoryMaterial == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdOrigin(int categoryId)
        {
            return await _context.Products.Where(p => p.IdCategoryOrigin == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdPrice(int categoryId)
        {
            return await _context.Products.Where(p => p.IdCategoryPrice == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdSex(int categoryId)
        {
            return await _context.Products.Where(p => p.IdCategorySex == categoryId).ToListAsync();
        }
    }
}