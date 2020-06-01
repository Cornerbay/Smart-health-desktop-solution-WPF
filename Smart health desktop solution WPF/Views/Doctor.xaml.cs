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
using static Smart_health_desktop_solution_WPF.Persistence;

namespace Smart_health_desktop_solution_WPF.Views
{
    /// <summary>
    /// Interaction logic for Doctor.xaml
    /// </summary>
    public partial class Doctor : UserControl
    {
        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con=null;
        private String table = "Doctor";
        private Persistence persistence = new Persistence();
        public Doctor()
        {
            setConnection();
            InitializeComponent();

            persistence.setComboBox(specializationList, "Specialization", 0,1);
            persistence.setComboBox(locationList, "Location", 0,1);
            setSearchComboBox();
        }



        private void setConnection()
        {
            con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }catch(Exception exp) 
            {
                MessageBox.Show("The connection seems to have failed due to:\n" + exp.ToString());
            }

        }

        private void updateDataGrid()
        {
            DataTable dt;
            if (string.IsNullOrWhiteSpace(searchTxt.Text))
            {
                dt = persistence.ReadTable(table);
                myDataGrid.ItemsSource = dt.DefaultView;
            }
            else if (!(columnComboBox.SelectedIndex > -1))
            {
                searchTxt.Visibility = Visibility.Visible;
                dt = persistence.ReadTable(table);
                myDataGrid.ItemsSource = dt.DefaultView;
            }
            else
            {
                dt = persistence.SearchTable(table, columnComboBox.SelectedItem.ToString(), searchTxt.Text);
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
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
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
                    cmd.Parameters.Add("@SpecializationID", SqlDbType.Int).Value = Int32.Parse((specializationList.SelectedItem as ComboboxItem).Value.ToString());
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 35).Value = firstNameTxt.Text;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 35).Value = lastNameTxt.Text;
                    cmd.Parameters.Add("@LocationID", SqlDbType.Int).Value = Int32.Parse((locationList.SelectedItem as ComboboxItem).Value.ToString());
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 128).Value = SecurePasswordHasher.Hash(passwordTxt.Text);

                    break;
                case "update":
                    msg = "Row Updated Successfully!";
                    cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Int32.Parse(doctorIDTxt.Text);
                    cmd.Parameters.Add("@SpecializationID", SqlDbType.Int).Value = Int32.Parse((specializationList.SelectedItem as ComboboxItem).Value.ToString());
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 35).Value = firstNameTxt.Text;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 35).Value = lastNameTxt.Text;
                    cmd.Parameters.Add("@LocationID", SqlDbType.Int).Value = Int32.Parse((locationList.SelectedItem as ComboboxItem).Value.ToString());

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
            catch (Exception expe) 
            {
                MessageBox.Show(expe.ToString());
            }
        }

        private void addBtnClick(object sender, RoutedEventArgs e)
        {
            String sql =    "INSERT INTO " + table + " (SpecializationID, FirstName, LastName, LocationID, Password) " +
                            "VALUES(@SpecializationID, @FirstName, @LastName, @LocationID, @Password);";
            this.AUD(sql, "add");
        }

        private void updateBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "UPDATE " + table + " SET " +
                            "SpecializationID = @SpecializationID," +
                            "FirstName=@Firstname, LastName=@LastName, LocationID = @LocationID " +
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
                specializationList.SelectedItem = dr["SpecializationID"];
                firstNameTxt.Text = dr["FirstName"].ToString();
                lastNameTxt.Text = dr["LastName"].ToString();
                //locationList.SelectedItem = dr["LocationID"].ToString();

                foreach(object listItem in specializationList.Items)
                {
                    if((listItem as ComboboxItem).Value.ToString().Equals(dr["SpecializationID"].ToString()))
                    {
                        specializationList.SelectedItem = listItem;
                        break;
                    }
                }

                foreach(object listItem in locationList.Items)
                {
                    if ((listItem as ComboboxItem).Value.ToString().Equals(dr["LocationID"].ToString()))
                    {
                        locationList.SelectedItem = listItem;
                        break;
                    }
                }

                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;

            }
        }

        private void deleteBtnClick(object sender, RoutedEventArgs e)
        {
            String sql =    "DELETE FROM " + table +
                            " WHERE DoctorID = @DoctorID;";
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
            specializationList.Text = "Choose Specialization";
            firstNameTxt.Text = "";
            lastNameTxt.Text = "";
            locationList.Text = "Choose Location";

            addBtn.IsEnabled = true;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
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
