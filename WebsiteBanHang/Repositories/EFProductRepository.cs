using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync();
            return await _context.Products
            .Include(p => p.Category) // Include thông tin về category
            .ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task EditAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await GetAllAsync(); // Trả về danh sách rỗng nếu không có tên được cung cấp
            }

            IQueryable<Product> query = _context.Products;
            query = query.Where(hh => hh.Name.ToLower().Contains(name.ToLower()));
            var result = await query.ToListAsync();

            return result;
        }
        public async Task<IEnumerable<Product>> SearchByCategory(int categoryId)
        {
            if (categoryId == null)
            {
                return await GetAllAsync(); // Trả về danh sách rỗng nếu không có thể loại được cung cấp
            }
            // Tìm kiếm sản phẩm theo thể loại, không phân biệt hoa thường
            var result = await _context.Products
                                       .Where(hh => hh.CategoryId == categoryId)
                                       .ToListAsync();
            return result; // Trả về danh sách sản phẩm tìm thấy theo thể loại
        }
        public async Task<IEnumerable<Product>> FilterByCategory(int categoryId)
        {
            return await _context.Products
                                 .Where(p => p.CategoryId == categoryId)
                                 .ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            // Logic để lấy danh sách các loại sản phẩm
            return await _context.Categories.ToListAsync();
        }
        public async Task<IEnumerable<Product>> FilterByPriceRange(decimal? from, decimal? to)
        {
            var query = _context.Products.AsQueryable();

            if (from.HasValue)
            {
                query = query.Where(p => p.Price >= from.Value);
            }
            if (to.HasValue)
            {
                query = query.Where(p => p.Price <= to.Value);
            }

            return await query.ToListAsync();
        }
    }
}
