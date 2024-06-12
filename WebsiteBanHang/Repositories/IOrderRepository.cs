using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task CreateAsync(Order hoaDon);
        Task EditAsync(Order hoaDon);
        Task DeleteAsync(int id);
        Task<IEnumerable<Order>> SearchByKhachHang(string name);
    }
}
