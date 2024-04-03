using System;
namespace do_an_ltweb.Models
{
	public interface ICategoryPrice
    {
        Task<IEnumerable<CategoryPrice>> GetAllAsync();
        Task<CategoryPrice> GetByIdAsync(int id);
        Task AddAsync(CategoryPrice category);
        Task UpdateAsync(CategoryPrice category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<CategoryPrice> GetByNameAsync(string name);
    }
}

