using System;
using System.Collections.Generic;
using System.Text;

namespace StudentResultSystem.Models
{
    internal abstract class Assessment
    {
        public string examinerName { get; private set; }

        public Assessment(string name)
        {
            examinerName = name;
        }

        public abstract decimal Score();
        
    }
}
