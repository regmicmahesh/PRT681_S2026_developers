using StudentResultSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentResultSystem.Models
{
    internal class ExamAssessment:Assessment
    {
        public decimal ObtainedMark { get; private set; }
        private readonly IScoreCalculator _scoreCalculator;

        public ExamAssessment(string name, decimal mark, IScoreCalculator scoreCalculator): base(name)
        {
            ObtainedMark = mark;
            _scoreCalculator = scoreCalculator;
        }

        public override decimal Score()
        {
            return _scoreCalculator.CalculateScore(ObtainedMark);
        }
    }
}
