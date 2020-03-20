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
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Location : UserControl
    {

        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con = null;
        private String table = "Location";
        public Location()
        {
            setConnection();
            InitializeComponent();
            Persistence persistence = new Persistence();

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
            Persistence persistence = new Persistence();
            DataTable dt = persistence.ReadTable(table);
            myDataGrid.ItemsSource = dt.DefaultView;
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
                    cmd.Parameters.Add("@Location", SqlDbType.VarChar, 64).Value = locationTxt.Text;
                    if (showLocationYes.IsChecked==true)
                    {
                        cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 1;
                    } if(showLocationNo.IsChecked==true)
                    {
                        cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 0;
                    }if(showLocationNo.IsChecked==false && showLocationYes.IsChecked == false)
                    {
                        cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 0;
                    }

                    break;
                case "update":
                    msg = "Row Updated Successfully!";
                    cmd.Parameters.Add("@Location", SqlDbType.VarChar, 64).Value = locationTxt.Text;
                    if (showLocationYes.IsChecked == true)
                    {
                        cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 1;
                    }
                    if (showLocationNo.IsChecked == true)
                    {
                        cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 0;
                    }
                    if (showLocationNo.IsChecked == false && showLocationYes.IsChecked == false)
                    {
                        cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 0;
                    }
                    break;
                case "delete":
                    msg = "Row Deleted Successfully!";

                    cmd.Parameters.Add("@Location", SqlDbType.VarChar, 64).Value = locationTxt.Text;

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
            //sjekk hvordan parameters add som står over fungerer.
            String sql = "INSERT INTO Location(Location, ShowLocation) " +
                            "VALUES(@Location, @ShowLocation);";
            this.AUD(sql, "add");
        }

        private void updateBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "UPDATE Location SET ShowLocation = @ShowLocation " +
                            "WHERE Location = @Location";
            this.AUD(sql, "update");
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                locationTxt.Text = dr["Location"].ToString();
                if (dr["ShowLocation"] == System.DBNull.Value)
                {
                    showLocationYes.IsChecked = false;
                    showLocationNo.IsChecked = false;
                }else if (Convert.ToInt32(dr["ShowLocation"]) == 1)
                {
                    showLocationYes.IsChecked = true;
                }else if(Convert.ToInt32(dr["ShowLocation"]) == 0)
                {
                    showLocationNo.IsChecked = true;
                
                }

                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;

            }
        }

        private void deleteBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "DELETE FROM Location " +
                            "WHERE Location = @Location";
            this.AUD(sql, "delete");
            this.resetAll();
        }

        private void resetBtnClick(object sender, RoutedEventArgs e)
        {
            resetAll();
        }

        private void resetAll()
        {
            locationTxt.Text = "";
            showLocationYes.IsChecked = false;
            showLocationNo.IsChecked = false;

            addBtn.IsEnabled = true;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }

        private void showLocationYes_Checked(object sender, RoutedEventArgs e)
        {
            showLocationNo.IsChecked = false;
        }

        private void showLocationNo_Checked(object sender, RoutedEventArgs e)
        {
            showLocationYes.IsChecked = false;
        }
    }
}
