using EmployeePayrollSystem.Models;
using EmployeePayrollSystem.Services;

List<Employee> employees = new List<Employee>();

employees.Add(new FullTimeEmployee("Deepjan", 4000m));
employees.Add( new CasualEmployee("Alex", 30m, 80m));

PayrollService payrollservice = new PayrollService();

foreach (Employee employee in employees)
{
    

    payrollservice.PrintPaySlip(employee);
}



Console.ReadLine();



