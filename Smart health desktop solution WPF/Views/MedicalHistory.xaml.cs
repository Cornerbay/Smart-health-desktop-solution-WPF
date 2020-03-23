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
    /// Interaction logic for MedicalHistory.xaml
    /// </summary>
    public partial class MedicalHistory : UserControl
    {
        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con = null;
        private String table = "MedicalHistory";
        public MedicalHistory()
        {
            setConnection();
            InitializeComponent();
            Persistence persistence = new Persistence();

            persistence.setComboBox(patientBirthNumberTxt, persistence.ReadTable("Patient"), 0);
            persistence.setComboBox(doctorIDTxt, persistence.ReadTable("Doctor"), 0);
            persistence.setComboBox(appointmentIDTxt, persistence.ReadTable("Appointment"), 0);
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
                    cmd.Parameters.Add("@PatientBirthNumber", SqlDbType.Char, 11).Value = patientBirthNumberTxt.SelectedItem;
                    cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Int32.Parse(doctorIDTxt.SelectedItem.ToString());
                    cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = Int32.Parse(appointmentIDTxt.SelectedItem.ToString());
                    cmd.Parameters.Add("@DoctorsNotes", SqlDbType.Text).Value = doctorsNotesTxt.Text;
                    break;
                case "update":
                    msg = "Row Updated Successfully!";
                    cmd.Parameters.Add("@MedicalHistoryID", SqlDbType.Int).Value = Int32.Parse(medicalHistoryIDTxt.Text);
                    cmd.Parameters.Add("@PatientBirthNumber", SqlDbType.Char, 11).Value = patientBirthNumberTxt.SelectedItem;
                    cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Int32.Parse(doctorIDTxt.SelectedItem.ToString());
                    cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = Int32.Parse(appointmentIDTxt.SelectedItem.ToString());
                    cmd.Parameters.Add("@DoctorsNotes", SqlDbType.Text).Value = doctorsNotesTxt.Text;
                    break;
                case "delete":
                    msg = "Row Deleted Successfully!";

                    cmd.Parameters.Add("@MedicalHistoryID", SqlDbType.Int).Value = Int32.Parse(medicalHistoryIDTxt.Text);

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
                            " (PatientBirthNumber, DoctorID, AppointmentID, DoctorsNotes) " +
                            "VALUES(@PatientBirthNumber, @DoctorID, @AppointmentID, @DoctorsNotes);";
            this.AUD(sql, "add");
        }

        private void updateBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "UPDATE " + table + " SET " +
                            "PatientBirthNumber=@PatientBirthNumber, DoctorID=@DoctorID, AppointmentID=@AppointmentID, " +
                            "DoctorsNotes=@DoctorsNotes " +
                            "WHERE MedicalHistoryID = @MedicalHistoryID";
            this.AUD(sql, "update");
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                medicalHistoryIDTxt.Text = dr["MedicalHistoryID"].ToString();
                patientBirthNumberTxt.Text = dr["PatientBirthNumber"].ToString();
                doctorIDTxt.Text = dr["DoctorID"].ToString();
                appointmentIDTxt.Text = dr["AppointmentID"].ToString();
                doctorsNotesTxt.Text = dr["DoctorsNotes"].ToString();

                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;

            }
        }

        private void deleteBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "DELETE FROM " + table +
                            " WHERE MedicalHisoryID = @MedicalHistoryID";
            this.AUD(sql, "delete");
            this.resetAll();
        }

        private void resetBtnClick(object sender, RoutedEventArgs e)
        {
            resetAll();
        }

        private void resetAll()
        {
            medicalHistoryIDTxt.Text = "Auto Assigned";
            patientBirthNumberTxt.Text = "Choose a patient";
            doctorIDTxt.Text = "Choose a doctor";
            appointmentIDTxt.Text = "Choose an appointment";
            doctorsNotesTxt.Text = "";

            addBtn.IsEnabled = true;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }


    }
}
