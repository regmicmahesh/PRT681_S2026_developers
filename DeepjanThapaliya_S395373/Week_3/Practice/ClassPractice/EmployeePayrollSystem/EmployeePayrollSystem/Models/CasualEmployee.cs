using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollSystem.Models
{
    public class CasualEmployee : Employee
    {
        private readonly decimal _hourlyRate;
        private readonly decimal _workedHours;

        public CasualEmployee(string name, decimal hourlyRate, decimal workedHours) : base(name)
        {
            if (hourlyRate < 0)
            {
                throw new ArgumentException("Hourly rate cannot be negative. ");
            }
            if (workedHours < 0)
            {
                throw new ArgumentException("Worked hours cannot be negative.");
            }
            _hourlyRate = hourlyRate;
            _workedHours = workedHours;
        }
        public override decimal CalculateGrossPay() 
        { 
            return (_hourlyRate * _workedHours);
        }
    }
}
