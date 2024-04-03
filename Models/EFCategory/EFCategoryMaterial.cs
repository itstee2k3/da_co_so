using Microsoft.EntityFrameworkCore;

namespace do_an_ltweb.Models;

public class EFCategoryMaterial : ICategoryMaterial
{
    private readonly ApplicationDbContext _context;
    public EFCategoryMaterial(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryMaterial> GetByNameAsync(string name)
    {
        return await _context.CategoryMaterials.FirstOrDefaultAsync(c => c.NameCategory == name);
    }

    public async Task<IEnumerable<CategoryMaterial>> GetAllAsync()
    {
        return await _context.CategoryMaterials.ToListAsync();
    }

    public async Task<CategoryMaterial> GetByIdAsync(int id)
    {
        return await _context.CategoryMaterials.FindAsync(id);
    }

    public async Task AddAsync(CategoryMaterial category)
    {
        _context.CategoryMaterials.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryMaterial category)
    {
        _context.CategoryMaterials.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.CategoryMaterials.FindAsync(id);
        _context.CategoryMaterials.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var category = await _context.CategoryMaterials.FindAsync(categoryId);
        if (category == null)
        {
            return null; // Trả về null nếu không tìm thấy danh mục
        }
        return await _context.Products.Where(p => p.IdCategoryMaterial == categoryId).ToListAsync();
    }
}