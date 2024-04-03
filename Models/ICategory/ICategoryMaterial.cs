using System;
namespace do_an_ltweb.Models
{
	public interface ICategoryMaterial
    {
        Task<IEnumerable<CategoryMaterial>> GetAllAsync();
        Task<CategoryMaterial> GetByIdAsync(int id);
        Task AddAsync(CategoryMaterial category);
        Task UpdateAsync(CategoryMaterial category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<CategoryMaterial> GetByNameAsync(string name);
    }
}

