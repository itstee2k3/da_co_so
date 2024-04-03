using Microsoft.EntityFrameworkCore;

namespace do_an_ltweb.Models;

public class EFCategorySex : ICategorySex
{
    private readonly ApplicationDbContext _context;
    public EFCategorySex(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategorySex> GetByNameAsync(string name)
    {
        return await _context.CategorySexes.FirstOrDefaultAsync(c => c.NameCategory == name);
    }

    public async Task<IEnumerable<CategorySex>> GetAllAsync()
    {
        return await _context.CategorySexes.ToListAsync();
    }

    public async Task<CategorySex> GetByIdAsync(int id)
    {
        return await _context.CategorySexes.FindAsync(id);
    }

    public async Task AddAsync(CategorySex category)
    {
        _context.CategorySexes.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategorySex category)
    {
        _context.CategorySexes.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.CategorySexes.FindAsync(id);
        _context.CategorySexes.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var category = await _context.CategorySexes.FindAsync(categoryId);
        if (category == null)
        {
            return null; // Trả về null nếu không tìm thấy danh mục
        }
        return await _context.Products.Where(p => p.IdCategorySex == categoryId).ToListAsync();
    }
}