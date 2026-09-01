using CleanCartSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCartSystem.Interfaces
{
    internal interface IDiscountCalculator
    {
        decimal GetDiscount(Customer customer, decimal subtotal);
    }
}
