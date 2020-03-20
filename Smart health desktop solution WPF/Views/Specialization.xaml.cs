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
    /// Interaction logic for Specialization.xaml
    /// </summary>
    public partial class Specialization : UserControl
    {

        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con = null;
        private String table = "Specialization";
        public Specialization()
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
                    cmd.Parameters.Add("@Specialization", SqlDbType.VarChar, 64).Value = specializationTxt.Text;
                    cmd.Parameters.Add("@SpecializationDescription", SqlDbType.VarChar, 256).Value = specializationDescTxt.Text;

                    break;
                case "update":
                    msg = "Row Updated Successfully!";
                    cmd.Parameters.Add("@Specialization", SqlDbType.VarChar, 64).Value = specializationTxt.Text;
                    cmd.Parameters.Add("@SpecializationDescription", SqlDbType.VarChar, 256).Value = specializationDescTxt.Text;

                    break;
                case "delete":
                    msg = "Row Deleted Successfully!";

                    cmd.Parameters.Add("@Specialization", SqlDbType.VarChar, 64).Value = specializationTxt.Text;

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
            String sql = "INSERT INTO Specialization(Specialization, SpecializationDescription) " +
                            "VALUES(@Specialization, @SpecializationDescription);";
            this.AUD(sql, "add");
        }

        private void updateBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "UPDATE Specialization SET SpecializationDescription = @SpecializationDescription" +
                            "WHERE Specialization = @Specialization";
            this.AUD(sql, "update");
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                specializationTxt.Text = dr["Specialization"].ToString();
                specializationDescTxt.Text = dr["SpecializationDescription"].ToString();

                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;

            }
        }

        private void deleteBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "DELETE FROM Specialization " +
                            "WHERE Specialization = @Specialization";
            this.AUD(sql, "delete");
            this.resetAll();
        }

        private void resetBtnClick(object sender, RoutedEventArgs e)
        {
            resetAll();
        }

        private void resetAll()
        {
            specializationTxt.Text = "";
            specializationDescTxt.Text = "";

            addBtn.IsEnabled = true;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }
    }
}

