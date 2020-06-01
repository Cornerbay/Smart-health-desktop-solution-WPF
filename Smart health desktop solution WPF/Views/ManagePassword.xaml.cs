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
    /// Interaction logic for ManagePassword.xaml
    /// </summary>
    public partial class ManagePassword : UserControl
    {
        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con = null;
        private String table;
        private Persistence persistence = new Persistence();
        public ManagePassword(string table)
        {
            setConnection();
            InitializeComponent();
            setSearchComboBox();
            this.table = table;
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
        }

        private void AUD(String sqlStatement)
        {
            String msg = "";
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlStatement;
            cmd.CommandType = CommandType.Text;
            if (!string.IsNullOrEmpty(userIDTxt.Text) && !string.IsNullOrEmpty(passwordTxt.Text))
            {
                switch (table)
                {
                    case "Doctor":
                        msg = "Password changed Successfully!";
                        cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Int32.Parse(userIDTxt.Text);
                        cmd.Parameters.Add("@Password", SqlDbType.VarChar, 128).Value = passwordTxt.Text;
                        break;
                    case "Patient":
                        msg = "Password changed Successfully!";
                        cmd.Parameters.Add("@BirthNumber", SqlDbType.Char).Value = userIDTxt.Text;
                        cmd.Parameters.Add("@Password", SqlDbType.VarChar, 128).Value = passwordTxt.Text;
                        break;
                    case "Admin":
                        msg = "Password changed Successfully!";
                        cmd.Parameters.Add("@AdminID", SqlDbType.Int).Value = Int32.Parse(userIDTxt.Text);
                        cmd.Parameters.Add("@Password", SqlDbType.VarChar, 128).Value = passwordTxt.Text;
                        break;
                }
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
                MessageBox.Show("Couldn't change password");
            }
        }

        private void changePasswordBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = null;
            switch (table)
            {
                case ("Doctor"):
                    sql = "UPDATE " + table + " SET " +
                                 "Password = @Password " +
                                 "WHERE DoctorID = @DoctorID;";
                    break;
                case ("Patient"):
                    sql = "UPDATE " + table + " SET " +
                                 "Password = @Password " +
                                 "WHERE BirthNumber = @BirthNumber;";
                    break;
                case ("Admin"):
                    sql = "UPDATE " + table + " SET " +
                                 "Password = @Password " +
                                 "WHERE AdminID = @AdminID;";
                    break;
            }

            if (sql != null)
            {
                this.AUD(sql);
            }
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                switch (table)
                {
                    case ("Doctor"):
                        userIDTxt.Text = dr["DoctorID"].ToString();
                        break;
                    case ("Patient"):
                        userIDTxt.Text = dr["BirthNumber"].ToString();
                        break;
                    case ("Admin"):
                        userIDTxt.Text = dr["AdminID"].ToString();
                        break;
                }

            }
        }

        private void resetBtnClick(object sender, RoutedEventArgs e)
        {
            resetAll();
        }

        private void resetAll()
        {
            userIDTxt.Text = "";
            passwordTxt.Text = "";

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
