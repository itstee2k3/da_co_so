using Microsoft.EntityFrameworkCore;

namespace do_an_ltweb.Models;

public class EFCategoryIrisColor : ICategoryIrisColor
{
    private readonly ApplicationDbContext _context;
    public EFCategoryIrisColor(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryIrisColor> GetByNameAsync(string name)
    {
        return await _context.CategoryIrisColors.FirstOrDefaultAsync(c => c.NameCategory == name);
    }

    public async Task<IEnumerable<CategoryIrisColor>> GetAllAsync()
    {
        return await _context.CategoryIrisColors.ToListAsync();
    }

    public async Task<CategoryIrisColor> GetByIdAsync(int id)
    {
        return await _context.CategoryIrisColors.FindAsync(id);
    }

    public async Task AddAsync(CategoryIrisColor category)
    {
        _context.CategoryIrisColors.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryIrisColor category)
    {
        _context.CategoryIrisColors.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.CategoryIrisColors.FindAsync(id);
        _context.CategoryIrisColors.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var category = await _context.CategoryIrisColors.FindAsync(categoryId);
        if (category == null)
        {
            return null; // Trả về null nếu không tìm thấy danh mục
        }
        return await _context.Products.Where(p => p.IdCategoryIrisColor == categoryId).ToListAsync();
    }
}