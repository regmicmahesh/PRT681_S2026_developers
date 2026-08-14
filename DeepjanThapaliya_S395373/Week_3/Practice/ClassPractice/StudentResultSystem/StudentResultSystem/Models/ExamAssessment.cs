using System;
using System.Collections.Generic;
using System.Text;

namespace StudentResultSystem.Models
{
    internal class ExamAssessment:Assessment
    {
        public decimal obtainedMark { get; private set; }

        public ExamAssessment(string name, decimal mark): base(name)
        {
            obtainedMark = mark;
        }

        public override decimal Score()
        {
            return obtainedMark;
        }
    }
}
