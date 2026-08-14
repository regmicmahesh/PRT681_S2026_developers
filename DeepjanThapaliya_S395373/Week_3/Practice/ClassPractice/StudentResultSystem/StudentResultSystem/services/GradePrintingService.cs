using StudentResultSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using StudentResultSystem.Interfaces;
using StudentResultSystem.ScoreCalculators;
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
            
            Console.WriteLine($"Student name: {_assessment.StudentName}");

            decimal totalScore = _assessment.Score();
            Console.WriteLine($"Score: {totalScore:F2}");
            GradeCalculator gradeCalculator = new GradeCalculator(totalScore);

            Console.WriteLine(gradeCalculator.CalculateGrade());
            Console.WriteLine();
        }
    }
}
