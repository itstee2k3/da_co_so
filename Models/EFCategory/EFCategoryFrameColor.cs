using Microsoft.EntityFrameworkCore;

namespace do_an_ltweb.Models;

public class EFCategoryFrameColor : ICategoryFrameColor
{
    private readonly ApplicationDbContext _context;
    public EFCategoryFrameColor(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryFrameColor> GetByNameAsync(string name)
    {
        return await _context.CategoryFrameColors.FirstOrDefaultAsync(c => c.NameCategory == name);
    }

    public async Task<IEnumerable<CategoryFrameColor>> GetAllAsync()
    {
        return await _context.CategoryFrameColors.ToListAsync();
    }

    public async Task<CategoryFrameColor> GetByIdAsync(int id)
    {
        return await _context.CategoryFrameColors.FindAsync(id);
    }

    public async Task AddAsync(CategoryFrameColor category)
    {
        _context.CategoryFrameColors.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryFrameColor category)
    {
        _context.CategoryFrameColors.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.CategoryFrameColors.FindAsync(id);
        _context.CategoryFrameColors.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var category = await _context.CategoryFrameColors.FindAsync(categoryId);
        if (category == null)
        {
            return null; // Trả về null nếu không tìm thấy danh mục
        }
        return await _context.Products.Where(p => p.IdCategoryFrameColor == categoryId).ToListAsync();
    }
}