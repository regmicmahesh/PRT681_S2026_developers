using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DreamSales.Models
{
    public class Cart
    {
        [Key]
        public string Username { get; set; }

        // Constructor to initialize the Cart with a username
        public Cart(string username)
        {
            CartItems = new List<CartItem>();
            Username = username;
        }

        // List of items in the cart
        public virtual IList<CartItem> CartItems { get; set; }

        // Optional: Total price of items in the cart (optional, could be calculated)
        public decimal TotalPrice
        {
            get
            {
                // Calculate total price based on cart items
                decimal total = 0;
                foreach (var item in CartItems)
                {
                    total += item.Price * item.Quantity;
                }
                return total;
            }
        }
    }
}
