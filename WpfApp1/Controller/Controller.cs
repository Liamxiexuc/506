using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAPSystem.Database;
using RAPSystem.Model;

namespace RAPSystem.Control
{
    class Controller
    {
        private List<Researcher> researchers;
        public List<Researcher> Researchers { get { return researchers; } set { } }

        private ObservableCollection<Researcher> viewableresearchers;
        public ObservableCollection<Researcher> Visibleresearchers { get { return viewableresearchers; } set { } }


   

        public Controller()
        {
            // get researcher list initially
            researchers = Database.Database.FetchBasicresearcherDetails();
            viewableresearchers = new ObservableCollection<Researcher>(researchers);

        }

        // get researcher detail
        public ObservableCollection<Researcher> ShowresearcherDetails()
        {
            return Visibleresearchers;
        }
        // get researcher detail
        public ObservableCollection<Researcher> Loadresearcher()
        {
            return Visibleresearchers;
        }

        public void SearchFilter(string Name)
        {
            var LinQ = from Researcher r in researchers
                           where (Name == null || Name.Length <= 0) || r.GivenName.ToLower().Contains(Name.ToLower()) || r.FamilyName.ToLower().Contains(Name.ToLower())

                           select r;
            viewableresearchers.Clear();
            //Converts the result of the LINQ expression to a List and then calls viewableResearcher.Add with each element of that list in turn
            LinQ.ToList().ForEach(viewableresearchers.Add);
        }

        public void LevelFilter(EmploymentLevel Level)
        {
            var LinQ = from Researcher r in researchers
                           where Level == EmploymentLevel.All || r.Level == Level
                           select r;
            viewableresearchers.Clear();

            LinQ.ToList().ForEach(viewableresearchers.Add);
        }

    }
}
