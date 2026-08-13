using System;
using System.Collections.Generic;
using System.Text;
using EmployeePayrollSystem.Interfaces;

namespace EmployeePayrollSystem.Calculators
{
    public class NoTaxCalculator:ITaxCalculator
    {
        public decimal CalculateTaxOnSalary(decimal grosspay)
        {
            return 0m;
        }
    }
}
