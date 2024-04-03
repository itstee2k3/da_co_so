using System;
namespace do_an_ltweb.Models
{
	public interface ICategoryIrisColor
    {
        Task<IEnumerable<CategoryIrisColor>> GetAllAsync();
        Task<CategoryIrisColor> GetByIdAsync(int id);
        Task AddAsync(CategoryIrisColor category);
        Task UpdateAsync(CategoryIrisColor category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<CategoryIrisColor> GetByNameAsync(string name);
    }
}

