using ECommerceLocal.Domain.Interfaces;
using ECommerceLocal.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceLocal.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
            return View(users);
        }

        // GET: Users/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var existingUser = await _userRepository.GetByEmailAsync(user.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Já existe um utilizador com este email.");
                return View(user);
            }

            await _userRepository.CreateAsync(user);

            TempData["Success"] = "Utilizador criado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Users/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, User user)
        {
            if (id != user.Id.ToString())
                return BadRequest();

            if (!ModelState.IsValid)
                return View(user);

            await _userRepository.UpdateAsync(id, user);

            TempData["Success"] = "Utilizador atualizado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Users/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _userRepository.DeleteAsync(id);

            TempData["Success"] = "Utilizador eliminado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/SearchByEmail
        public async Task<IActionResult> SearchByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return RedirectToAction(nameof(Index));

            var user = await _userRepository.GetByEmailAsync(email);

            var result = new List<User>();

            if (user != null)
                result.Add(user);

            ViewBag.Email = email;

            return View("Index", result);
        }

    }
}
