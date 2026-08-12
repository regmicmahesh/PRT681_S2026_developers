using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollSystem.Calculators
{
    public class SimpleTaxCalculator
    {
        public decimal CalculateTax(decimal grossPay)
        {
            return grossPay * 0.1m;
        }
    }
}
