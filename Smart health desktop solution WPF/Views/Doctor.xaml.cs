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
    /// Interaction logic for Doctor.xaml
    /// </summary>
    public partial class Doctor : UserControl
    {
        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con=null;
        public Doctor()
        {
            setConnection();
            InitializeComponent();
        }

        private void setConnection()
        {
            con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }catch(Exception exp) { }

        }

        private void updateDataGrid()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT DoctorID, Specialization, FirstName, LastName, Location FROM Doctor ORDER BY LastName DESC;";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            Console.WriteLine(dt);
            dt.Load(dr);
            myDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //con.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.updateDataGrid();
        }

        private void AUD(String sqlStatement, String operation)
        {
            String msg = "";
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlStatement;
            cmd.CommandType = CommandType.Text;
            
            switch (operation)
            {
                case "add":
                    msg = "Row Inserted Successfully!";
                    cmd.Parameters.Add("@Specialization", SqlDbType.VarChar, 64).Value = specializationTxt.Text;
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 35).Value = firstNameTxt.Text;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 35).Value = lastNameTxt.Text;
                    cmd.Parameters.Add("@Location", SqlDbType.VarChar, 64).Value = locationTxt.Text;

                    break;
                case "update":
                    msg = "Row Updated Successfully!";
                    cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Int32.Parse(doctorIDTxt.Text);
                    cmd.Parameters.Add("@Specialization", SqlDbType.VarChar, 64).Value = specializationTxt.Text;
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 35).Value = firstNameTxt.Text;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 35).Value = lastNameTxt.Text;
                    cmd.Parameters.Add("@Location", SqlDbType.VarChar, 64).Value = locationTxt.Text;

                    break;
                case "delete":
                    msg = "Row Deleted Successfully!";

                    cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Int32.Parse(doctorIDTxt.Text);

                    break;
            }
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updateDataGrid();
                }
            }
            catch (Exception expe) { }
        }

        private void addBtnClick(object sender, RoutedEventArgs e)
        {
            //sjekk hvordan parameters add som står over fungerer.
            String sql =    "INSERT INTO Doctor(Specialization, FirstName, LastName, Location) " +
                            "VALUES(@Specialization, @FirstName, @LastName, @Location);";
            this.AUD(sql, "add");
        }

        private void updateBtnClick(object sender, RoutedEventArgs e)
        {
            String sql =    "UPDATE Doctor SET Specialization = @Specialization," +
                            "FirstName=@Firstname, LastName=@LastName, Location = @Location " +
                            "WHERE DoctorID = @DoctorID";
            this.AUD(sql, "update");
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                doctorIDTxt.Text = dr["DoctorID"].ToString();
                specializationTxt.Text = dr["Specialization"].ToString();
                firstNameTxt.Text = dr["FirstName"].ToString();
                lastNameTxt.Text = dr["LastName"].ToString();
                locationTxt.Text = dr["Location"].ToString();

                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;

            }
        }

        private void deleteBtnClick(object sender, RoutedEventArgs e)
        {
            String sql =    "DELETE FROM Doctor " +
                            "WHERE DoctorID = @DoctorID";
            this.AUD(sql, "delete");
            this.resetAll();
        }

        private void resetBtnClick(object sender, RoutedEventArgs e)
        {
            resetAll();
        }

        private void resetAll()
        {
            doctorIDTxt.Text = "Auto assigned";
            specializationTxt.Text = "";
            firstNameTxt.Text = "";
            lastNameTxt.Text = "";
            locationTxt.Text = "";

            addBtn.IsEnabled = true;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }
    }
}
