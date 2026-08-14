using StudentResultSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentResultSystem.services
{
    internal class GradePrintingService
    {
        private readonly Assessment _assessment;

        internal GradePrintingService(Assessment accessment)
        {
            _assessment = accessment;
        }

        public void printGrade()
        {
            Console.WriteLine($"Student name: {_assessment.examinerName}");

            decimal totalScore = _assessment.Score();
            Console.WriteLine($"Score: {totalScore:F2}");
            if (totalScore > 80)
            {
                Console.WriteLine($"Grade: A");
            }else if(totalScore > 60 && totalScore < 80)
            {
                Console.WriteLine($"Grade: B");
            }else Console.WriteLine($"Grade: F");

            Console.WriteLine();
        }
    }
}
