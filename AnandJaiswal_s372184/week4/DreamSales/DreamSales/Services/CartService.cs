using Microsoft.EntityFrameworkCore;
using DreamSales.Data;
using DreamSales.Models;

namespace DreamSales.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Retrieve the current user's cart
        public Cart GetCart()
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (username == null) { return null; }

            var cart = _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Item).FirstOrDefault(c => c.Username == username);
            if (cart == null)
            {
                cart = new Cart(username);
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            return cart;
        }

        // Add an item to the cart
        public void AddItem(Item product, int quantity)
        {
            Cart cart = GetCart();
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Item.Id == product.Id);

            if (cartItem == null)
            {
                cartItem = new CartItem { Item = product, Cart = cart, Quantity = quantity };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            _context.SaveChanges();
        }

        // Get the total number of items in the cart
        public int CartCount()
        {
            var cart = GetCart();
            return cart.CartItems.Sum(i => i.Quantity);
        }

        // Remove an item from the cart, or reduce its quantity
        public int RemoveItem(int itemId, int qty = 1)
        {
            Cart cart = GetCart();
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Item.Id == itemId);

            if (cartItem == null) return 0;

            cartItem.Quantity -= qty;
            if (cartItem.Quantity <= 0)
            {
                cart.CartItems.Remove(cartItem);
            }

            _context.SaveChanges();
            return cartItem.Quantity;
        }

        // Get the total quantity of a specific item in the cart
        public int TotalItem(int itemId)
        {
            Cart cart = GetCart();
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Item.Id == itemId);
            return cartItem?.Quantity ?? 0;
        }

        // Clear all items from the cart
        public void ClearCart()
        {
            var cart = GetCart();
            if (cart != null)
            {
                cart.CartItems.Clear();
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
        }
    }
}
