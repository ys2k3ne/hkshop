using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task CreateAsync(Category category);
        Task EditAsync(Category category);
        Task DeleteAsync(int id);
    }
}
