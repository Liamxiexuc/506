﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAPSystem.Model
{
    class Student : Researcher
    {
        public string Degree { set; get; }

        public int SupervisorId { set; get; }

        public string SupervisorName { set; get; }

        public int TotalSupervisions { set; get; }

        public override string ToString()
        {
            return FullName;
        }
    }
}
