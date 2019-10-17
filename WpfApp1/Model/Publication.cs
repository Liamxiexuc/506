using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAPSystem.Model
{
    // type
    //public enum Type { Poor, Below, MeetingMinimum, StarPerformers };
    enum PublicationType
    {
        Conference,
        Journal,
        Other
    }

    class Publication
    {
        // publication id
        public string DOI { get; set; }
        // publication name
        public string title { get; set; }
        // authors
        public string author { get; set; }
        // year
        public int year { get; set; }
       
        public PublicationType type { get; set; }
        // citeAs
        public string citeAs { get; set; }
        // availabledate
        public DateTime availableDate { get; set; }
        // Age
        public int age { get; set; }

        public override string ToString()
        {
            return year + "   " + title;

        }
    }
}
