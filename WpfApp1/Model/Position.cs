using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAPSystem.Model
{
    class Position
    {
        public string positionName { set; get; }
        public DateTime startDate { set; get; }
        public DateTime endDate { set; get; }

        public override string ToString()
        {
            return startDate.ToString("yyyy-MM-dd") + "   " + endDate.ToString("yyyy-MM-dd") + "   " + positionName;
        }
    }
}
