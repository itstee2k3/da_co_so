using System;
namespace do_an_ltweb.Models
{
	public interface ICategoryBrand
    {
        Task<IEnumerable<CategoryBrand>> GetAllAsync();
        Task<CategoryBrand> GetByIdAsync(int id);
        Task AddAsync(CategoryBrand category);
        Task UpdateAsync(CategoryBrand category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<CategoryBrand> GetByNameAsync(string name);
    }
}

