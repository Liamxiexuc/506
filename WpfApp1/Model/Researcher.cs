using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAPSystem.Model
{
    enum EmploymentLevel
    {
        All,
        Student,
        A,
        B,
        C,
        D,
        E
    }

    enum Campus
    {
        Hobart,
        Launceston,
        CradleCoast
    }
    enum Type
    {
        Student,
        Staff
    }
    class Researcher
    {
        // researcher id
        public int ID { get; set; }
        // full name
        public string FullName { get; set; }
        // family name
        public string FamilyName { get; set; }
        // family name
        public string GivenName { get; set; }
        // researcher title
        public string Title { get; set; }
        // school
        public string School { get; set; }
        // campus
        public Campus Campus { get; set; }
        // telephone number
        public string Unit { get; set; }
        // email
        public string Email { get; set; }
        // room
        public string Photo { get; set; }
        //photo
        public Type Type { get; set; }

        public EmploymentLevel Level { get; set; }

        public override string ToString()
        {
            // return full name
            return GivenName + " " + FamilyName + " (" + Title + ")";
        }
    }
}
