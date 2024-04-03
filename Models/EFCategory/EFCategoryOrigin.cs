using Microsoft.EntityFrameworkCore;

namespace do_an_ltweb.Models;

public class EFCategoryOrigin : ICategoryOrigin
{
    private readonly ApplicationDbContext _context;
    public EFCategoryOrigin(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryOrigin> GetByNameAsync(string name)
    {
        return await _context.CategoryOrigins.FirstOrDefaultAsync(c => c.NameCategory == name);
    }

    public async Task<IEnumerable<CategoryOrigin>> GetAllAsync()
    {
        return await _context.CategoryOrigins.ToListAsync();
    }

    public async Task<CategoryOrigin> GetByIdAsync(int id)
    {
        return await _context.CategoryOrigins.FindAsync(id);
    }

    public async Task AddAsync(CategoryOrigin category)
    {
        _context.CategoryOrigins.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryOrigin category)
    {
        _context.CategoryOrigins.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.CategoryOrigins.FindAsync(id);
        _context.CategoryOrigins.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var category = await _context.CategoryOrigins.FindAsync(categoryId);
        if (category == null)
        {
            return null; // Trả về null nếu không tìm thấy danh mục
        }
        return await _context.Products.Where(p => p.IdCategoryOrigin == categoryId).ToListAsync();
    }
}