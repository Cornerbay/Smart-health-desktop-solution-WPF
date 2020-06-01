using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace Smart_health_desktop_solution_WPF.Views
{
    /// <summary>
    /// Interaction logic for MyAppointments.xaml
    /// </summary>
    public partial class MyAppointments : UserControl
    {

        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con = null;
        private String table = "Appointment";
        private Persistence persistence = new Persistence();
        private string userID;
        public MyAppointments(string iD)
        {
            setConnection();
            InitializeComponent();
            setSearchComboBox();
            userID = iD;
        }

        private void setConnection()
        {
            con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception exp)
            {
                MessageBox.Show("The connection seems to have failed due to:\n" + exp.ToString());
            }

        }

        private void updateDataGrid()
        {
            DataTable dt;
            if (string.IsNullOrWhiteSpace(searchTxt.Text))
            {
                dt = persistence.ReadMyAppointmentsTable(table, userID);
                myDataGrid.ItemsSource = dt.DefaultView;
            }
            else if (!(columnComboBox.SelectedIndex > -1))
            {
                searchTxt.Visibility = Visibility.Visible;
                dt = persistence.ReadMyAppointmentsTable(table, userID);
                myDataGrid.ItemsSource = dt.DefaultView;
            }
            else
            {
                dt = persistence.SearchMyAppointmentsTable(table, columnComboBox.SelectedItem.ToString(), searchTxt.Text, userID);
                myDataGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            con.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.updateDataGrid();
        }

        private void setSearchComboBox()
        {
            DataTable dt = persistence.GetColumnNames(table);

            foreach (DataRow row in dt.Rows)
            {
                columnComboBox.Items.Add(row[0].ToString());
            }
        }

        private void searchUpdate(object sender, TextChangedEventArgs e)
        {
            this.updateDataGrid();
        }

        private void columnBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((columnComboBox.SelectedIndex > -1))
            {
                searchTxt.Visibility = Visibility.Visible;
            }
        }
    }
}
