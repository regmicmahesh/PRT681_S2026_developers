using System;


Console.Write("Name: ");
string name = Console.ReadLine();

Console.Write("Hourly Pay Rate: ");
if (!double.TryParse(Console.ReadLine(), out double hourlyPayRate)) {
    Console.WriteLine("Invalid Hourly Pay Rate");
    return 1;
}

Console.Write("Total Hours Worked: ");
if (!int.TryParse(Console.ReadLine(), out int totalHoursWorked))
{
    Console.WriteLine("Invalid Total Hours Worked");
    return 1;
}

double grossSalary = hourlyPayRate * totalHoursWorked;
double taxWithheld = 0.2 * grossSalary;
double netSalary = grossSalary - taxWithheld;

Console.WriteLine("------------------------------");
Console.WriteLine($"Gross Pay: ${grossSalary:F2}");
Console.WriteLine($"Tax Witheld: ${taxWithheld:F2}");
Console.WriteLine($"Net Pay: ${netSalary:F2}");
Console.WriteLine("------------------------------");

return 0;
