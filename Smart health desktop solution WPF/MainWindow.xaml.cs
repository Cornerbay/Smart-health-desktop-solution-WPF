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

            /*            string connectionString = "SERVER=smart-health-solution.mysql.database.azure.com;DATABASE=app2000;UID=designerkaktus@smart-health-solution;PASSWORD=Vieralleveldigsunne1;";

                        MySqlConnection connection = new MySqlConnection(connectionString);

                        MySqlCommand cmd = new MySqlCommand("select*from patient", connection);
                        connection.Open();
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                        connection.Close();*/




            Persistence persistence = new Persistence();
            
            DataTable dt = persistence.readTable("patient");

            PrintValues(dt);

            dtGrid.DataContext = dt;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hashtable htPatientRow = new Hashtable()
            {
                {"Table", "patient"},
                {"BirthNumber", Int64.Parse(Birthdate.Text)},
                {"FirstName", "fornavn"},
                {"LastName", "etternavn"},
                {"Adress", "addresse"},
                {"Birthdate","2001-01-01"},
                {"AuthorizationLevel", 1}
            };
            Persistence persistence = new Persistence();
            persistence.addPatient(htPatientRow);
        }

        private void PrintValues(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine(row);
                foreach (DataColumn column in table.Columns)
                {
                    Console.WriteLine(row[column]);
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WindowAddNew newWindow = new WindowAddNew();
            newWindow.Show();
        }
    }
}
