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
        private Persistence persistence = new Persistence();

        public Location()
        {
            setConnection();
            InitializeComponent();
            setSearchComboBox();

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
                    cmd.Parameters.Add("@LocationName", SqlDbType.VarChar, 64).Value = locationNameTxt.Text;
                    if (showLocationComboBox.SelectedIndex > -1)
                    {
                        if (showLocationComboBox.SelectedItem.ToString().Equals("Yes"))
                        {
                            cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 1;
                        } if(showLocationComboBox.SelectedItem.ToString().Equals("No"))
                        {
                            cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 0;
                        }else
                        {
                            cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 0;
                        }
                    }

                    break;
                case "update":
                    msg = "Row Updated Successfully!";
                    cmd.Parameters.Add("@LocationID", SqlDbType.Int).Value = Int32.Parse(locationIDTxt.Text);
                    cmd.Parameters.Add("@LocationName", SqlDbType.VarChar, 64).Value = locationNameTxt.Text;
                    if (showLocationComboBox.SelectedIndex > -1)
                    {
                        if (showLocationComboBox.SelectedValue.ToString().Equals("Yes"))
                        {
                            cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 1;
                        }
                        else if (showLocationComboBox.SelectedValue.ToString().Equals("No"))
                        {
                            cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 0;
                        }
                        else
                        {
                            cmd.Parameters.Add("@ShowLocation", SqlDbType.TinyInt).Value = 0;
                        }
                    }
                    break;
                case "delete":
                    msg = "Row Deleted Successfully!";

                    cmd.Parameters.Add("@LocationID", SqlDbType.Int).Value = Int32.Parse(locationIDTxt.Text);

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
            String sql = "INSERT INTO " + table + " (LocationName, ShowLocation) " +
                            "VALUES(@LocationName, @ShowLocation);";
            this.AUD(sql, "add");
        }

        private void updateBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "UPDATE " + table + " SET " +
                            "LocationName = @LocationName, " +
                            "ShowLocation = @ShowLocation " +
                            "WHERE LocationID = @LocationID;";
            this.AUD(sql, "update");
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                locationIDTxt.Text = dr["LocationID"].ToString();
                locationNameTxt.Text = dr["LocationName"].ToString();
                if (dr["ShowLocation"] == System.DBNull.Value)
                {
                    showLocationComboBox.SelectedIndex = 1;
                }else if (Convert.ToInt32(dr["ShowLocation"]) == 1)
                {
                    showLocationComboBox.SelectedIndex = 0;
                }
                else if(Convert.ToInt32(dr["ShowLocation"]) == 0)
                {
                    showLocationComboBox.SelectedIndex = 1;
                }

                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;

            }
        }

        private void deleteBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "DELETE FROM Location " +
                            "WHERE LocationID = @LocationID";
            this.AUD(sql, "delete");
            this.resetAll();
        }

        private void resetBtnClick(object sender, RoutedEventArgs e)
        {
            resetAll();
        }

        private void resetAll()
        {
            locationIDTxt.Text = "Auto Assigned";
            locationNameTxt.Text = "";
            showLocationComboBox.Text = "--Choose an option--";

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
