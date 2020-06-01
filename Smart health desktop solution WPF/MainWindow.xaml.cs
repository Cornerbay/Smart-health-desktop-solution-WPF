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
using System.Windows.Media.Animation;

namespace Smart_health_desktop_solution_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string userID;
        private string userTable;
        public MainWindow(string user)
        {
            InitializeComponent();
            userTxtBlock.Text = user;
        }

        public MainWindow(string user, string iD, string table)
        {
            InitializeComponent();
            userTxtBlock.Text = user;
            userID = iD;

            if (table.Equals("Doctor"))
            {
                myAppointmentsBtn.Visibility = Visibility.Visible;
            }

        }



        private void doctorBtnClicked(object sender, RoutedEventArgs e)
        {
            frontPageStackPanel.Visibility = Visibility.Hidden;
            DataContext = new Doctor();
        }

        private void specializationBtnClicked(object sender, RoutedEventArgs e)
        {
            frontPageStackPanel.Visibility = Visibility.Hidden;
            DataContext = new Specialization();
        }

        private void locationBtnClicked(object sender, RoutedEventArgs e)
        {
            frontPageStackPanel.Visibility = Visibility.Hidden;
            DataContext = new Location();
        }

        private void patientBtnClicked(object sender, RoutedEventArgs e)
        {
            frontPageStackPanel.Visibility = Visibility.Hidden;
            DataContext = new Patient();
        }

        private void appointmentBtnClicked(object sender, RoutedEventArgs e)
        {
            frontPageStackPanel.Visibility = Visibility.Hidden;
            DataContext = new Appointment();
        }

        private void medicalHistoryBtnClicked(object sender, RoutedEventArgs e)
        {
            frontPageStackPanel.Visibility = Visibility.Hidden;
            DataContext = new MedicalHistory();
        }

        private void adminBtnClicked(object sender, RoutedEventArgs e)
        {
            frontPageStackPanel.Visibility = Visibility.Hidden;
            DataContext = new Admin();
        }
        private void myAppointmentsBtnClicked(object sender, RoutedEventArgs e)
        {
            frontPageStackPanel.Visibility = Visibility.Hidden;
            DataContext = new Views.MyAppointments(userID);
        }

        private void tableManagementBtnClicked(object sender, RoutedEventArgs e)
        {
            if(dataManagementStackPanel.Visibility == Visibility.Collapsed)
            {
                dataManagementStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                dataManagementStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void exitBtnClicked(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}
