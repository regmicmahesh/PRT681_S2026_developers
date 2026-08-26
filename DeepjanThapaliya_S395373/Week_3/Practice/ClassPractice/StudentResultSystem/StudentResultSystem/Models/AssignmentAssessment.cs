using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using StudentResultSystem.Interfaces;
using StudentResultSystem.ScoreCalculators;

namespace StudentResultSystem.Models
{
    internal class AssignmentAssessment:Assessment
    {
        public decimal TotalObtainedMark { get; private set; }
        private readonly IScoreCalculator _scoreCalculator;
        public AssignmentAssessment(string name, decimal mark,IScoreCalculator scoreCalculator) : base(name)
        {
            if (mark < 0m)
            {
                throw new ArgumentException("Students mark cannot be negative.");
            }else if (mark > 40)
            {
                throw new ArgumentException("Students mark cannot be greater than 40.");
            } else
                TotalObtainedMark = mark;
                _scoreCalculator = scoreCalculator;
        }

        public override decimal Score()
        {
            return _scoreCalculator.CalculateScore(TotalObtainedMark);

        }
    }
}
