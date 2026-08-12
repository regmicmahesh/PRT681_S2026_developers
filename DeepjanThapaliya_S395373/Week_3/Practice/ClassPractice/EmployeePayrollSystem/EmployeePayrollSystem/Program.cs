using EmployeePayrollSystem.Models;
using EmployeePayrollSystem.Services;
using EmployeePayrollSystem.Calculators;

List<Employee> employees = new List<Employee>();

employees.Add(new FullTimeEmployee("Deepjan", 4000m));
employees.Add( new CasualEmployee("Alex", 30m, 80m));

PayrollService payrollservice = new PayrollService(new SimpleTaxCalculator());

foreach (Employee employee in employees)
{
    

    payrollservice.PrintPayslip(employee);
}



Console.ReadLine();



