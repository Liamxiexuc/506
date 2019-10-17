using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RAPSystem.Control;
using RAPSystem.Database;
using RAPSystem.Model;


namespace RAPSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller controller = new Controller();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Researcher> BasicResearcher = controller.Researchers;
            ResearcherListBox.ItemsSource = BasicResearcher;
        }

        private void SearchBT_Click(object sender, RoutedEventArgs e)
        {
            string Text = InputName.Text;
            if(Text == "")
            {
                ResearcherListBox.ItemsSource = null;
            }
            else
            {
                controller.SearchFilter(Text);
                ResearcherListBox.ItemsSource = controller.Loadresearcher();
            }
        }

        private void ByLevel_Loaded(object sender, RoutedEventArgs e)
        {
            ByLevel.ItemsSource = System.Enum.GetNames(typeof(EmploymentLevel));
        }

        private void ByLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox ByLevelCB = (ComboBox)sender;
            //Get selected Level(Enum type)
            EmploymentLevel level = (EmploymentLevel)Enum.Parse(typeof(EmploymentLevel), ByLevelCB.SelectedItem.ToString(), false);
            controller.LevelFilter(level);
            ResearcherListBox.ItemsSource = controller.Loadresearcher();
        }

        private void ResearcherListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Researcher researcher = (Researcher)ResearcherListBox.SelectedItem;
            if (researcher == null) { return; };
            // reset UI
            PreviousPositionLB.ItemsSource = null;
            SupervisionNameListBox.ItemsSource = null;
            string OBJType = Convert.ToString(researcher.Type);
            switch (OBJType)
            {
                case "Student" :
                    Student student = new Student();
                    student = Database.Database.FetchStudentDetails(researcher.ID);
                    student.SupervisorName = Database.Database.FetchSupervisorName(researcher.ID);
                    student.TotalPublication = Database.Database.FetchTotalPublication(researcher.ID);
                    // Connect Student Data to UI
                    ResearcherDetails1.DataContext = student;
                    ResearcherDetails2.DataContext = student;
                    ResearcherDetails3.DataContext = student;
                    break;
                case "Staff":
                    Staff staff = new Staff();
                    staff = Database.Database.FetchStaffDetails(researcher.ID);
                    staff.TotalPublication = Database.Database.FetchTotalPublication(researcher.ID);
                    staff.TotalSupervisions = Database.Database.FetchTotalSupervisions(researcher.ID);
                    staff.ThreeYearAverage = Database.Database.FetchThreeYearAverage(researcher.ID);
                    staff.Performance = Database.Database.FetchPerformance(researcher.Level, staff.ThreeYearAverage);
                    // Connect Staff Data to UI
                    ResearcherDetails1.DataContext = staff;
                    ResearcherDetails2.DataContext = staff;
                    ResearcherDetails3.DataContext = staff;
                    List<Position> previousPosition = Database.Database.FetchPreviousPosition(researcher.ID);
                    PreviousPositionLB.ItemsSource = previousPosition;
                    break;
                default:
                    return;
            }

            List<Publication> listPublication = Database.Database.FetchPublications(researcher.ID);
            // Connect Publicationlist to UI
            PublicationListBox.ItemsSource = listPublication;
            researcher.PublicationList = listPublication;

            //reset publication details
            PublicationDetails1.DataContext = null;
            PublicationDetails2.DataContext = null;
        }

        private void PublicationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Publication publicationOBJ = (Publication)PublicationListBox.SelectedItem;
            if (publicationOBJ == null) { return; }        
            Publication publication = new Publication();
            publication = Database.Database.FetchPublicationDetails(publicationOBJ.DOI);
            // Connect Publication Data to UI
            PublicationDetails1.DataContext = publication;
            PublicationDetails2.DataContext = publication;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Researcher researcher = (Researcher)ResearcherListBox.SelectedItem;
            SupervisionNameListBox.ItemsSource = Database.Database.FetchSupervisionName(researcher.ID);
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            ComboBox SortComboBox = (ComboBox)sender;
            List<Publication> newOrder = new List<Publication>();
            Researcher Researcher = (Researcher)ResearcherListBox.SelectedItem;
            string orderType = SortComboBox.SelectedItem.ToString();
            newOrder = controller.PublicationSort(Researcher.PublicationList, orderType);
            // update UI
            PublicationListBox.ItemsSource = newOrder;
        }
    }
}
