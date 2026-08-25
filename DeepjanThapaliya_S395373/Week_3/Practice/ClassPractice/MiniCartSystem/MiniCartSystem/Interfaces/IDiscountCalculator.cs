using System;
using System.Collections.Generic;
using System.Text;
using MiniCartSystem.Models;

namespace MiniCartSystem.Interfaces
{
    internal interface IDiscountCalculator
    {
        public decimal CalculateDiscount(Customer customer, decimal lineTotal);
    }
}
