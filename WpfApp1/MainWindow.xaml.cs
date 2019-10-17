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
    }
}
