using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Areas.Admin.Controllers {
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductController(IProductRepository productRepository,

        ICategoryRepository categoryRepository)

        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile

        imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                await _productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            if (!Directory.Exists("wwwroot/images"))
            {
                Directory.CreateDirectory("wwwroot/images");
            }
            var savePath = Path.Combine("wwwroot/images", image.FileName);

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName;
        }

        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl");
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingProduct = await
                _productRepository.GetByIdAsync(id);
                if (imageUrl == null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                else
                {
                    // Lưu hình ảnh mới

                    product.ImageUrl = await SaveImage(imageUrl);

                }
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
                await _productRepository.EditAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            await _productRepository.DeleteAsync(product.Id);
            return RedirectToAction(nameof(Index));
        }

        /*public IActionResult SearchByName(string productName)
        {
            var foundProducts = _productRepository.FindAll(p => p.Name.Contains(productName));
            return View(foundProducts);
        }*/
    }
}
