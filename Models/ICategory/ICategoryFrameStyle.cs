using System;
namespace do_an_ltweb.Models
{
	public interface ICategoryFrameStyle
    {
        Task<IEnumerable<CategoryFrameStyle>> GetAllAsync();
        Task<CategoryFrameStyle> GetByIdAsync(int id);
        Task AddAsync(CategoryFrameStyle category);
        Task UpdateAsync(CategoryFrameStyle category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<CategoryFrameStyle> GetByNameAsync(string name);
    }
}

