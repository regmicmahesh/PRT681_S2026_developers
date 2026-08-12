using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollSystem.Models
{
    public abstract class Employee
    {
        public string Name { get; set; }
        public Employee(string name)
        {
            Name = name;
        }

        public abstract decimal CalculateGrossPay();

    }
}
