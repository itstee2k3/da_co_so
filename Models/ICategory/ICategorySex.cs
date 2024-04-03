using System;
namespace do_an_ltweb.Models
{
	public interface ICategorySex
    {
        Task<IEnumerable<CategorySex>> GetAllAsync();
        Task<CategorySex> GetByIdAsync(int id);
        Task AddAsync(CategorySex category);
        Task UpdateAsync(CategorySex category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<CategorySex> GetByNameAsync(string name);
    }
}

