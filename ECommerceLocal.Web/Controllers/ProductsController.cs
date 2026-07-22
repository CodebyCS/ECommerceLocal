using ECommerceLocal.Domain.Interfaces;
using ECommerceLocal.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ECommerceLocal.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Search(string name)
        {
            var products = await _productRepository.SearchByNameAsync(name);

            ViewBag.Search = name;

            return View("Index", products);
        }

        // GET: Products/Details/ID
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            await _productRepository.CreateAsync(product);

            TempData["Success"] = "Produto criado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/ID
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Edit/ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Product product)
        {
            if (id != product.Id.ToString())
                return BadRequest();

            if (!ModelState.IsValid)
                return View(product);

            await _productRepository.UpdateAsync(id, product);

            TempData["Success"] = "Produto atualizado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/ID
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Delete/ID
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _productRepository.DeleteAsync(id);

            TempData["Success"] = "Produto eliminado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/SearchCategory
        public async Task<IActionResult> SearchCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return RedirectToAction(nameof(Index));

            var products = await _productRepository.GetByCategoryAsync(category);

            ViewBag.Category = category;

            return View("Index", products);
        }

        // GET: Products/SearchPrice
        public async Task<IActionResult> SearchPrice(decimal minPrice, decimal maxPrice)
        {
            var products = await _productRepository.GetByPriceAsync(minPrice, maxPrice);

            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View("Index", products);
        }
    }
}
