using System;
using System.Collections.Generic;
using System.Text;

namespace StudentResultSystem.Interfaces
{
    internal interface IScoreCalculator
    {
        public decimal CalculateScore(decimal obtainedMark);
        
    }
}
