using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayrollSystem.Models
{
    public class CasualEmployee : Employee
    {
        private readonly decimal _hourlyRate;
        private readonly decimal _workedHour;

        public CasualEmployee(string name, decimal hourlyRate, decimal workedHour) : base(name)
        {
            _hourlyRate = hourlyRate;
            _workedHour = workedHour;
        }
        public override decimal CalculateGrossPay() 
        { 
            return (_hourlyRate * _workedHour);
        }
    }
}
