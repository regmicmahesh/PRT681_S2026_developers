using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCartSystem.Models
{
    internal class CartItem
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }

        public CartItem(Product product, int quantity)
        {
            if(quantity < 1)
            {
                throw new ArgumentException("Cart Model Error: Product quantity " +
                    "must be greater than zero.");
            }

            Product = product ?? throw new ArgumentNullException(nameof(product), "There is no product.");

            Quantity = quantity;
        }

        public decimal GetLineTotal()
        {
            return Product.Price * Quantity;
        }

        public override string ToString()
        {
            return $"{Product.ProductName} - QTY: {Quantity} - Unit Price: ${Product.Price} - LineTotal: ${GetLineTotal()}";
        }

        public void IncreaseQuantity(int quantity)
        {
            if (quantity < 1)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            Quantity += quantity;
        }

        public void DecreaseQuantity()
        {

            Quantity -= 1;
        }
    }
}
