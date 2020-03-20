using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Collections;
using Smart_health_desktop_solution_WPF.ViewModels;

namespace Smart_health_desktop_solution_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void doctorBtnClicked(object sender, RoutedEventArgs e)
        {
            DataContext = new Doctor();
        }

        private void specializationBtnClicked(object sender, RoutedEventArgs e)
        {
            DataContext = new Specialization();
        }

        private void locationBtnClicked(object sender, RoutedEventArgs e)
        {
            DataContext = new Location();
        }

        private void patientBtnClicked(object sender, RoutedEventArgs e)
        {
            DataContext = new Patient();
        }
    }
}
