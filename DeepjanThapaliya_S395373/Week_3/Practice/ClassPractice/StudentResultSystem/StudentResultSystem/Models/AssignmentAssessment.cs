using System;
using System.Collections.Generic;
using System.Text;

namespace StudentResultSystem.Models
{
    internal class AssignmentAssessment:Assessment
    {
        public decimal ObtainedMark { get; private set; }

        public AssignmentAssessment(string name, decimal mark) : base(name)
        {
            ObtainedMark = mark;
        }

        public override decimal Score()
        {
            return ObtainedMark;
        }
    }
}
