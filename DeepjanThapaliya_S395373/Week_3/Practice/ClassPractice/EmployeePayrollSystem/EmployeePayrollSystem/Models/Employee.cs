using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollSystem.Models
{
    public abstract class Employee
    {
        public string Name { get; private set; }
        public Employee(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Employee name cannot be empty. ");
            }
            Name = name;
        }

        public abstract decimal CalculateGrossPay();

    }
}
