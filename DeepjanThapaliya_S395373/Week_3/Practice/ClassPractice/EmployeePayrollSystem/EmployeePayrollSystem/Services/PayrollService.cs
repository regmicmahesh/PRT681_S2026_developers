using System;
using System.Collections.Generic;
using System.Text;
using EmployeePayrollSystem.Models;
namespace EmployeePayrollSystem.Services
{
    public class PayrollService
    {
        

        public void PrintPaySlip(Employee employee)
        { 
            decimal grossPay = employee.CalculateGrossPay();
            Console.WriteLine($"Employee: {employee.Name}");
            Console.WriteLine($"Gross Salary: {grossPay}");
            decimal taxOnSalary = grossPay * 0.1m;
            Console.WriteLine($"Tax: {taxOnSalary}");
            Console.WriteLine($"Net payable Salary: {grossPay - taxOnSalary}");
            Console.WriteLine();
        }
    }
}
