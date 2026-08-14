using System;
using System.Collections.Generic;
using System.Text;
using StudentResultSystem.ScoreCalculators;

namespace StudentResultSystem.Models
{
    internal class AssignmentAssessment:Assessment
    {
        public decimal TotalObtainedMark { get; private set; }

        public AssignmentAssessment(string name, decimal mark) : base(name)
        {
            TotalObtainedMark = mark;
        }

        public override decimal Score()
        {
            
            return TotalObtainedMark;
        }
    }
}
