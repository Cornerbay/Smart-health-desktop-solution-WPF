using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Patient.xaml
    /// </summary>
    public partial class Patient : UserControl
    {
        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con = null;
        private String table = "Patient";
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        public Patient()
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
                    cmd.Parameters.Add("@BirthNumber", SqlDbType.Char, 11).Value = birthNumberTxt.Text;
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 35).Value = firstNameTxt.Text;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 35).Value = lastNameTxt.Text;
                    cmd.Parameters.Add("@Address", SqlDbType.VarChar, 128).Value = addressTxt.Text;
                    cmd.Parameters.Add("@Birthdate", SqlDbType.Date).Value = birthdateDatePicker.SelectedDate;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 128).Value = emailTxt.Text;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 128).Value = passwordTxt.Text;
                    cmd.Parameters.Add("@Phone", SqlDbType.Char, 8).Value = phoneTxt.Text;
                    cmd.Parameters.Add("@PostalCode", SqlDbType.Char, 4).Value = postalCodeTxt.Text;
                    cmd.Parameters.Add("@Town", SqlDbType.VarChar, 65).Value = townTxt.Text;

                    break;
                case "update":
                    msg = "Row Updated Successfully!";
                    cmd.Parameters.Add("@BirthNumber", SqlDbType.Char, 11).Value = birthNumberTxt.Text;
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 35).Value = firstNameTxt.Text;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 35).Value = lastNameTxt.Text;
                    cmd.Parameters.Add("@Address", SqlDbType.VarChar, 128).Value = addressTxt.Text;
                    cmd.Parameters.Add("@Birthdate", SqlDbType.Date).Value = birthdateDatePicker.SelectedDate;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 128).Value = emailTxt.Text;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 128).Value = passwordTxt.Text;
                    cmd.Parameters.Add("@Phone", SqlDbType.Char, 8).Value = phoneTxt.Text;
                    cmd.Parameters.Add("@PostalCode", SqlDbType.Char, 4).Value = postalCodeTxt.Text;
                    cmd.Parameters.Add("@Town", SqlDbType.VarChar, 65).Value = townTxt.Text;

                    break;
                case "delete":
                    msg = "Row Deleted Successfully!";

                    cmd.Parameters.Add("@BirthNumber", SqlDbType.Char, 11).Value = birthNumberTxt.Text;

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
            String sql = "INSERT INTO " + table +
                            " (BirthNumber, FirstName, LastName, Address, Birthdate, Email, Password, Phone, PostalCode, Town) " +
                            "VALUES(@BirthNumber, @FirstName, @LastName, @Address, @Birthdate, @Email, @Password, @Phone, @PostalCode, @Town);";
            this.AUD(sql, "add");
        }

        private void updateBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "UPDATE "+ table + " SET " +
                            "FirstName=@Firstname, LastName=@LastName, Address=@Address, " +
                            "Birthdate=@BirthDate, Email=@Email, Password=@Password," +
                            "Phone=@Phone, PostalCode=@PostalCode, Town=@Town " +
                            "WHERE BirthNumber = @BirthNumber";
            this.AUD(sql, "update");
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                birthNumberTxt.Text = dr["BirthNumber"].ToString();
                firstNameTxt.Text = dr["FirstName"].ToString();
                lastNameTxt.Text = dr["LastName"].ToString();
                addressTxt.Text = dr["Address"].ToString();
                birthdateDatePicker.SelectedDate = DateTime.Parse(dr["Birthdate"].ToString());
                emailTxt.Text = dr["Email"].ToString();
                passwordTxt.Text = dr["Password"].ToString();
                phoneTxt.Text = dr["Phone"].ToString();
                postalCodeTxt.Text = dr["PostalCode"].ToString();
                townTxt.Text = dr["Town"].ToString();

                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;

            }
        }

        private void deleteBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "DELETE FROM " + table +
                            " WHERE BirthNumber = @BirthNumber";
            this.AUD(sql, "delete");
            this.resetAll();
        }

        private void resetBtnClick(object sender, RoutedEventArgs e)
        {
            resetAll();
        }

        private void resetAll()
        {
            birthNumberTxt.Text = "";
            firstNameTxt.Text = "";
            lastNameTxt.Text = "";
            addressTxt.Text = "";
            birthdateDatePicker.SelectedDate = null;
            emailTxt.Text = "";
            passwordTxt.Text = "";
            phoneTxt.Text = "";
            postalCodeTxt.Text = "";
            townTxt.Text = "";

            addBtn.IsEnabled = true;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }

    private void onlyNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void onlyNumber_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

    }
}
