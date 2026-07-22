using ECommerceLocal.Domain.Interfaces;
using ECommerceLocal.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceLocal.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            return View(orders);
        }

        // DETALHES
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // CREATE (GET)
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new Order());
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(order);
            }

            decimal total = 0;

            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId.ToString());

                if (product == null)
                    continue;

                item.UnitPrice = product.Price;

                total += product.Price * item.Quantity;
            }

            order.Total = total;
            order.Date = DateTime.Now;

            await _orderRepository.CreateAsync(order);

            TempData["Success"] = "Encomenda criada com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            await LoadDropdowns();

            return View(order);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Order order)
        {
            if (id != order.Id.ToString())
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(order);
            }

            decimal total = 0;

            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId.ToString());

                if (product == null)
                    continue;

                item.UnitPrice = product.Price;

                total += product.Price * item.Quantity;
            }

            order.Total = total;

            await _orderRepository.UpdateAsync(id, order);

            TempData["Success"] = "Encomenda atualizada com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _orderRepository.DeleteAsync(id);

            TempData["Success"] = "Encomenda eliminada.";

            return RedirectToAction(nameof(Index));
        }

        // MÉTODO AUXILIAR
        private async Task LoadDropdowns()
        {
            var users = await _userRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            ViewBag.Users = users.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });

            ViewBag.Products = products.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name} - {x.Price:C}"
            });
        }
    }
}
}
