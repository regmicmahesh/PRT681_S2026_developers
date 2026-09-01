using CleanCartSystem.Interfaces;
using CleanCartSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCartSystem.Calculators
{
    internal class SimpleDiscountCalculator: IDiscountCalculator
    {
        public decimal GetDiscount(Customer customer, decimal subtotal)
        {
            Customer checkedCustomer = customer ?? throw new ArgumentNullException("No customer found.");

            if (subtotal < 0)
            {
                throw new ArgumentException("Subtotal cannot be negative.");
            }
            return checkedCustomer.IsPremiumCustomer ? (subtotal * 0.1m) : 0m;
        }
    }
}
