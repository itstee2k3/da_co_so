using System;
namespace do_an_ltweb.Models
{
	public interface ICategoryFrameColor
    {
        Task<IEnumerable<CategoryFrameColor>> GetAllAsync();
        Task<CategoryFrameColor> GetByIdAsync(int id);
        Task AddAsync(CategoryFrameColor category);
        Task UpdateAsync(CategoryFrameColor category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<CategoryFrameColor> GetByNameAsync(string name);
    }
}

