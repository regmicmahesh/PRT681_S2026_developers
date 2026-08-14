using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollSystem.Models
{
    public class FullTimeEmployee : Employee
    {
        private readonly decimal _monthlySalary;
        public FullTimeEmployee(string name, decimal monthlySalary) : base(name)
        {
            if(monthlySalary < 0)
            {
                throw new ArgumentException("Montly salary cannot be negative. ");
            }
            _monthlySalary = monthlySalary;
        }
        public override decimal CalculateGrossPay()
        {
            return _monthlySalary;
        }
    }
}
