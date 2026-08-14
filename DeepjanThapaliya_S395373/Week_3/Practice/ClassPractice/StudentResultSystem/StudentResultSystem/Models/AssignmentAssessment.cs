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
            TotalObtainedMark = mark;
            _scoreCalculator = scoreCalculator;
        }

        public override decimal Score()
        {
            return _scoreCalculator.CalculateScore(TotalObtainedMark);

        }
    }
}
