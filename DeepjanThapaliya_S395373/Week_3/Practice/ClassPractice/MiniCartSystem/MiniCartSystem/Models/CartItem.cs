using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCartSystem.Models
{
    internal class CartItem
    {
        public Product Product { get; private set; }
        public decimal Quantity { get; private set; }

        public CartItem(Product item, decimal quantity)
        {
            if (quantity < 1)
            {
                throw new ArgumentException("Cart item Error: Quantity cannot be less then 1.");
            }

            if(item == null)
            {
                throw new ArgumentNullException("Cart item Error: There is no product in cart.");
            }
            Product = item;
            Quantity = quantity;
        }

        public  decimal GetLineTotal()
        {
            decimal lineTotal = Product.Price * Quantity;
            return lineTotal;
        }
    }
}
