using Microsoft.EntityFrameworkCore;

namespace do_an_ltweb.Models;

public class EFCategoryFrameStyle : ICategoryFrameStyle
{
    private readonly ApplicationDbContext _context;
    public EFCategoryFrameStyle(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryFrameStyle> GetByNameAsync(string name)
    {
        return await _context.CategoryFrameStyles.FirstOrDefaultAsync(c => c.NameCategory == name);
    }

    public async Task<IEnumerable<CategoryFrameStyle>> GetAllAsync()
    {
        return await _context.CategoryFrameStyles.ToListAsync();
    }

    public async Task<CategoryFrameStyle> GetByIdAsync(int id)
    {
        return await _context.CategoryFrameStyles.FindAsync(id);
    }

    public async Task AddAsync(CategoryFrameStyle category)
    {
        _context.CategoryFrameStyles.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryFrameStyle category)
    {
        _context.CategoryFrameStyles.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.CategoryFrameStyles.FindAsync(id);
        _context.CategoryFrameStyles.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var category = await _context.CategoryFrameStyles.FindAsync(categoryId);
        if (category == null)
        {
            return null; // Trả về null nếu không tìm thấy danh mục
        }
        return await _context.Products.Where(p => p.IdCategoryFrameStyle == categoryId).ToListAsync();
    }
}