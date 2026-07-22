using ECommerceLocal.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceLocal.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public ReportsController(
            IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        // Dashboard de relatórios
        public IActionResult Index()
        {
            return View();
        }

        // =====================================================
        // Lookup entre Orders e Products
        // =====================================================
        public async Task<IActionResult> OrdersWithProducts()
        {
            var orders = await _orderRepository.GetOrdersWithProductsAsync();

            return View(orders);
        }

        // =====================================================
        // Total gasto por cliente ($group)
        // =====================================================
        public async Task<IActionResult> TotalSpentPerCustomer()
        {
            var result = await _orderRepository.GetTotalSpentPerCustomerAsync();

            return View(result);
        }

        // =====================================================
        // Produtos por categoria
        // =====================================================
        [HttpGet]
        public IActionResult SearchCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                ModelState.AddModelError("", "Informe uma categoria.");
                return View();
            }

            var products = await _productRepository.GetByCategoryAsync(category);

            return View("CategoryResult", products);
        }

        // =====================================================
        // Produtos por intervalo de preço
        // =====================================================
        [HttpGet]
        public IActionResult SearchPrice()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchPrice(decimal minPrice, decimal maxPrice)
        {
            if (minPrice > maxPrice)
            {
                ModelState.AddModelError("", "Preço mínimo não pode ser maior que o máximo.");
                return View();
            }

            var products = await _productRepository.GetByPriceAsync(minPrice, maxPrice);

            return View("PriceResult", products);
        }
    }
}
