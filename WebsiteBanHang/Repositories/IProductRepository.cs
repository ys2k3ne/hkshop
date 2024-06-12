using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task CreateAsync(Product product);
        Task EditAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> SearchByName(string name);
        Task<IEnumerable<Product>> SearchByCategory(int categoryId);
        Task<IEnumerable<Product>> FilterByCategory(int categoryId);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<Product>> FilterByPriceRange(decimal? from, decimal? to);
    }
}
