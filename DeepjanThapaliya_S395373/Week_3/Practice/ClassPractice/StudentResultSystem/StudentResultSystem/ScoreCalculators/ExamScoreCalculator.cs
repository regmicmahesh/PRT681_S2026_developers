using StudentResultSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentResultSystem.ScoreCalculators
{
    internal class ExamScoreCalculator:IScoreCalculator
    {
        public decimal CalculateScore(decimal obtainedMark)
        {
            decimal totalMark = 400m;
            return ((obtainedMark / totalMark)*100m);
        }
    }
}
