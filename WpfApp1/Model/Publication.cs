using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAPSystem.Model
{
    // type
   // public enum PubType { Poor, Below, MeetingMinimum, StarPerformers };

    class Publication
    {
        // publication id
        public string DOI { get; set; }
        // publication name
        public string title { get; set; }
        // authors
        public string authors { get; set; }
        // year
        public DateTime year { get; set; }
       
        public string type { get; set; }
        // citeAs
        public string citeAs { get; set; }
        // availabledate
        public DateTime availableDate { get; set; }
        // Age
        public int age { get; set; }
    }
}
