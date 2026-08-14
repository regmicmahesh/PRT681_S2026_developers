using StudentResultSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentResultSystem.ScoreCalculators
{
    internal class AssessmentScoreCalculator : IScoreCalculator
    {
        public decimal CalculateScore(decimal markObtained)
        {
            decimal totalMark = 40m;
            return markObtained / totalMark * 100m;
        }
    }
}
