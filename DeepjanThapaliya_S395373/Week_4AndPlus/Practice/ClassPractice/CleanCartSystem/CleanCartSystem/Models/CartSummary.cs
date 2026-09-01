using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCartSystem.Models
{
    internal class CartSummary
    {
        public Customer Customer { get; private set; }
        public IReadOnlyList<CartItem> Items { get; private set; }
        public decimal Subtotal { get; private set; }
        public decimal Discount { get; private set; }
        public decimal FinalTotal { get; private set; }
        public CartSummary(
            Customer customer,
            IReadOnlyList<CartItem> items,
            decimal subtotal,
            decimal discount,
            decimal finalTotal)
        {
            Customer = customer ?? throw new ArgumentNullException("Customer is not found");
            Items = items ?? throw new ArgumentNullException("Cart is Empty");
            if (subtotal < 0)
            {
                throw new ArgumentException("Subtotal cannot be negative.");
            }

            if (discount < 0)
            {
                throw new ArgumentException("Discount cannot be negative.");
            }

            if (finalTotal < 0)
            {
                throw new ArgumentException("Final total cannot be negative.");
            }
            Subtotal = subtotal;
            Discount = discount;
            FinalTotal = finalTotal;
        }
    }
}
