using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamSales.Data;
using DreamSales.Models;
using DreamSales.Services;

namespace DreamSales.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;

        public CartController(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // GET: Cart
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            ViewBag.totalcost = cart.CartItems.Sum(ci => ci.Item.Price * ci.Quantity);
            return View(cart.CartItems);
        }

        // GET: Cart/Add/5
        public async Task<IActionResult> Add(int id)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            if (item.Quantity > 0)
            {
                item.Quantity--;
                await _context.SaveChangesAsync();
                _cartService.AddItem(item, 1);
            }

            return RedirectToAction("Index");
        }

        // GET: Cart/Minus/5
        public async Task<IActionResult> Minus(int id)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            int whatsLeft = _cartService.RemoveItem(id, 1);

            item.Quantity++;
            await _context.SaveChangesAsync();

            if (whatsLeft > 0)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", "Items");
        }

        // GET: Cart/Remove/5
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            int total = _cartService.TotalItem(id);

            _cartService.RemoveItem(id, total);

            item.Quantity += total;
            await _context.SaveChangesAsync();

            if (_cartService.CartCount() > 0)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", "Items");
        }

        // GET: Cart/Checkout
        public ActionResult Checkout()
        {
            _cartService.ClearCart();
            return RedirectToAction("Index", "Items");
        }
    }
}
