using System;
using System.Collections.Generic;
using System.Text;
using EmployeePayrollSystem.Interfaces;

namespace EmployeePayrollSystem.Calculators
{
    public class SimpleTaxCalculator : ITaxCalculator
    {
        
        public decimal CalculateTaxOnSalary(decimal grossPay)
        {
            return grossPay * 0.1m;
        }
    }
}
