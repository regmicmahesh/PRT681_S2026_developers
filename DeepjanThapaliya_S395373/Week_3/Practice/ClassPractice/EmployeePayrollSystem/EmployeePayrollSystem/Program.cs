using EmployeePayrollSystem.Models;
using EmployeePayrollSystem.Services;
using EmployeePayrollSystem.Calculators;


try
{
    List<Employee> employees = new List<Employee>();
    employees.Add(new FullTimeEmployee("Deepjan", 4000m));
    employees.Add(new CasualEmployee("Alex", 30m, 80m));

    PayrollService payrollservice = new PayrollService(new NoTaxCalculator());

    foreach (Employee employee in employees)
    {


        payrollservice.PrintPayslip(employee);
    }
}catch(ArgumentException ex)
{
    Console.WriteLine($"Validation Error: {ex.Message}");
}




Console.ReadLine();



