using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAPSystem.Model
{
    class Staff : Researcher
    {
        public double ThreeYearAverage { set; get; }

        public string Performance { set; get; }

        public int SupervisionsAmount { set; get; }
    }
}
