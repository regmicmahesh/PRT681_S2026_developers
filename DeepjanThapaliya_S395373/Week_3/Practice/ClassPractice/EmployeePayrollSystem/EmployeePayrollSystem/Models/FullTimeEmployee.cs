using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollSystem.Models
{
    public class FullTimeEmployee : Employee
    {
        private decimal _monthlySalary;
        public FullTimeEmployee(string name, decimal monthlySalary) : base(name)
        {
            _monthlySalary = monthlySalary;
        }
        public override decimal CalculateGrossPay()
        {
            return _monthlySalary;
        }
    }
}
