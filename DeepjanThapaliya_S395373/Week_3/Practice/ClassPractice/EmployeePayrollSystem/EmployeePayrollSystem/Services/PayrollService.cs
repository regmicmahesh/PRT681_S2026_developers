using EmployeePayrollSystem.Calculators;
using EmployeePayrollSystem.Models;
namespace EmployeePayrollSystem.Services;

public class PayrollService
{

    private readonly SimpleTaxCalculator _taxCalculator;

    public PayrollService(SimpleTaxCalculator taxCalculator)
    {
        _taxCalculator = taxCalculator;
    }
    public void PrintPayslip(Employee employee)
    { 
        decimal grossPay = employee.CalculateGrossPay();
        Console.WriteLine($"Employee: {employee.Name}");
        Console.WriteLine($"Gross Salary: {grossPay}");

        decimal taxOnSalary = _taxCalculator.CalculateTax(grossPay);
        Console.WriteLine($"Tax: {taxOnSalary}");

        decimal netSalary = grossPay - taxOnSalary;
        Console.WriteLine($"Net payable Salary: {netSalary}");
        Console.WriteLine();
    }
}