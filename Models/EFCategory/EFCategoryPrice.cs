using Microsoft.EntityFrameworkCore;

namespace do_an_ltweb.Models;

public class EFCategoryPrice : ICategoryPrice
{
    private readonly ApplicationDbContext _context;
    public EFCategoryPrice(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryPrice> GetByNameAsync(string name)
    {
        return await _context.CategoryPrices.FirstOrDefaultAsync(c => c.NameCategory == name);
    }

    public async Task<IEnumerable<CategoryPrice>> GetAllAsync()
    {
        return await _context.CategoryPrices.ToListAsync();
    }

    public async Task<CategoryPrice> GetByIdAsync(int id)
    {
        return await _context.CategoryPrices.FindAsync(id);
    }

    public async Task AddAsync(CategoryPrice category)
    {
        _context.CategoryPrices.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryPrice category)
    {
        _context.CategoryPrices.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.CategoryPrices.FindAsync(id);
        _context.CategoryPrices.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var category = await _context.CategoryPrices.FindAsync(categoryId);
        if (category == null)
        {
            return null; // Trả về null nếu không tìm thấy danh mục
        }
        return await _context.Products.Where(p => p.IdCategoryPrice == categoryId).ToListAsync();
    }
}