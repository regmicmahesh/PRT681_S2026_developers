using System;
using System.Collections.Generic;
using System.Text;

namespace StudentResultSystem.ScoreCalculators
{
    internal class GradeCalculator
    {
        private readonly decimal _score;

        internal GradeCalculator(decimal score)
        {
            _score = score;
        }

        public string CalculateGrade()
        {
            if (_score > 80)
            {
                return($"Grade: A");
            }
            else if (_score > 60 && _score < 80)
            {
                return ($"Grade: B");
            }
            else return ($"Grade: F");
        }
    }
}
