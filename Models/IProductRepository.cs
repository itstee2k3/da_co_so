using System;
namespace do_an_ltweb.Models
{
	public interface IProductRepository
	{
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetByCategoryIdBrand(int id);
        Task<IEnumerable<Product>> GetByCategoryIdFrameColor(int id);
        Task<IEnumerable<Product>> GetByCategoryIdFrameStyle(int id);
        Task<IEnumerable<Product>> GetByCategoryIdIrisColor(int id);
        Task<IEnumerable<Product>> GetByCategoryIdMaterial(int id);
        Task<IEnumerable<Product>> GetByCategoryIdOrigin(int id);
        Task<IEnumerable<Product>> GetByCategoryIdPrice(int id);
        Task<IEnumerable<Product>> GetByCategoryIdSex(int id);

        //Task<IEnumerable<Product>> GetByCategoryIdsAsync(IEnumerable<int> categoryIds);
    }
}

