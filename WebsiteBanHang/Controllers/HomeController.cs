using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;
using X.PagedList;

namespace WebsiteBanHang.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index(string name, int? page, int? categoryId, decimal? from, decimal? to)
        {
            int pageSize = 12; // Số lượng sản phẩm trên mỗi trang
            int pageNumber = page ?? 1; // Số trang hiện tại
                                        // Trong phương thức Index của Controller
            ViewBag.Name = name;
            ViewBag.CategoryId = categoryId;
            ViewBag.From = from;
            ViewBag.To = to;

            // Lấy danh sách loại sản phẩm từ cơ sở dữ liệu
            var categories = await _productRepository.GetCategoriesAsync();
            ViewBag.Categories = categories; // Thêm này để sử dụng trong View

            IEnumerable<Product> products = await _productRepository.GetAllAsync();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }
            if (!string.IsNullOrEmpty(name))
            {
                products = await _productRepository.SearchByName(name);
            }
            if (from.HasValue && to.HasValue)
            {
                products = await _productRepository.FilterByPriceRange(from.Value, to.Value);
            }

            // Sử dụng ToPagedList để tạo một danh sách sản phẩm đã phân trang
            var pagedProducts = products.ToPagedList(pageNumber, pageSize);

            return View(pagedProducts); // Trả về view kèm theo danh sách sản phẩm đã phân trang
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
