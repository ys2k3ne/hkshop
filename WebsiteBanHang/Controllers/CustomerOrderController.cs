using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Controllers
{
    public class CustomerOrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IEnumerable<Order> products = await _orderRepository.SearchByKhachHang(userId);
            return View(products);
        }
        public CustomerOrderController(IOrderRepository orderRepository, ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task<IActionResult> Display(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var hoaDon = await _orderRepository.GetByIdAsync(id);
            if (hoaDon == null || hoaDon.UserId != userId)
            {
                return NotFound();
            }
            var orderDetails = _context.OrderDetails.Where(od => od.OrderId == id).ToList();
            return View(orderDetails);
        }
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var hoaDon = await _orderRepository.GetByIdAsync(id);
            if (hoaDon == null || hoaDon.UserId != userId)
            {
                return NotFound();
            }
            _orderRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
