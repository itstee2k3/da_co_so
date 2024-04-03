using System;
namespace do_an_ltweb.Models
{
	public interface ICategoryOrigin
    {
        Task<IEnumerable<CategoryOrigin>> GetAllAsync();
        Task<CategoryOrigin> GetByIdAsync(int id);
        Task AddAsync(CategoryOrigin category);
        Task UpdateAsync(CategoryOrigin category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<CategoryOrigin> GetByNameAsync(string name);
    }
}

