using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public EFOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync();
            return await _context.Orders
            .ToListAsync();
        }
        public async Task<Order> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task CreateAsync(Order product)
        {
            _context.Orders.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task EditAsync(Order product)
        {
            _context.Orders.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Order>> SearchByKhachHang(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return await GetAllAsync(); // Trả về danh sách rỗng nếu không có tên được cung cấp
            }

            var orders = await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
            return orders;
        }
    }
}
