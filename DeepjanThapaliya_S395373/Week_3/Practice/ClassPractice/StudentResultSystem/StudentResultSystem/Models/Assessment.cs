using System;
using System.Collections.Generic;
using System.Text;

namespace StudentResultSystem.Models
{
    internal abstract class Assessment
    {
        public string StudentName { get; private set; }

        public Assessment(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name Cannot be Empty.");
            } else
            StudentName = name;
        }

        public abstract decimal Score();
        
    }
}
