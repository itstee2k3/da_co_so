using Microsoft.EntityFrameworkCore;

namespace do_an_ltweb.Models;

public class EFCategoryBrand : ICategoryBrand
{
    private readonly ApplicationDbContext _context;
    public EFCategoryBrand(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryBrand> GetByNameAsync(string name)
    {
        return await _context.CategoryBrands.FirstOrDefaultAsync(c => c.NameCategory == name);
    }

    public async Task<IEnumerable<CategoryBrand>> GetAllAsync()
    {
        return await _context.CategoryBrands.ToListAsync();
    }

    public async Task<CategoryBrand> GetByIdAsync(int id)
    {
        return await _context.CategoryBrands.FindAsync(id);
    }

    public async Task AddAsync(CategoryBrand category)
    {
        _context.CategoryBrands.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryBrand category)
    {
        _context.CategoryBrands.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.CategoryBrands.FindAsync(id);
        _context.CategoryBrands.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var category = await _context.CategoryBrands.FindAsync(categoryId);
        if (category == null)
        {
            return null; // Trả về null nếu không tìm thấy danh mục
        }
        return await _context.Products.Where(p => p.IdCategoryBrand == categoryId).ToListAsync();
    }
}