using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollSystem.Interfaces
{
    public interface ITaxCalculator
    {
        public decimal CalculateTaxOnSalary(decimal grosspay);
    }
}
