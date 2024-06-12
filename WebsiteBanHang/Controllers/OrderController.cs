using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;
        public OrderController(IOrderRepository productRepository, ApplicationDbContext
        categoryRepository)
        {
            _orderRepository = productRepository;
            _context = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            var order = await _orderRepository.GetAllAsync();
            return View(order);
        }
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Order order)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.CreateAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        public async Task<IActionResult> Update(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _orderRepository.EditAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Category cate)
        {
            var order = await _orderRepository.GetByIdAsync(cate.Id);
            if (order != null)
            {
                _orderRepository.DeleteAsync(cate.Id);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
