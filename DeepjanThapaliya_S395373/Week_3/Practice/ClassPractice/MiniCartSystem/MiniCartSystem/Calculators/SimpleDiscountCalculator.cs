using MiniCartSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MiniCartSystem.Models;

namespace MiniCartSystem.Calculators
{
    internal class SimpleDiscountCalculator:IDiscountCalculator
    {
        public decimal CalculateDiscount(Customer customer, decimal subTotal)
        {
            if (subTotal < 0)
            {
                throw new ArgumentException("Discount Error: SubTotal cannot be less then 0.");
            }
            if (customer.IsPremium)
            {
                return subTotal * 0.1m;
            }
            else
                return 0m;
        }
    }
}
