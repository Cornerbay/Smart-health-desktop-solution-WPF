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

    public partial class Appointment : UserControl
    {
        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con = null;
        private String table = "Appointment";
        private Persistence persistence = new Persistence();
        public Appointment()
        {
            setConnection();
            InitializeComponent();
            loadTimeComboBoxes();
            loadStatusComboBox();

            persistence.setComboBox(patientBirthNumberTxt, "Patient" , 0, 1, 2);
            persistence.setComboBox(doctorIDTxt, persistence.ReadTable("Doctor"), 0);
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
            else if(!(columnComboBox.SelectedIndex > -1))
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
            string time = appointmentTimeHourPicker.SelectedItem.ToString() + ":" + appointmentTimeMinutesPicker.SelectedItem.ToString();
            object status = appointmentStatusComboBox.SelectedItem;

            switch (operation)
            {
                case "add":
                    msg = "Row Inserted Successfully!";
                    cmd.Parameters.Add("@PatientBirthNumber", SqlDbType.Char, 11).Value = patientBirthNumberTxt.SelectedItem;
                    cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Int32.Parse(doctorIDTxt.SelectedItem.ToString());
                    cmd.Parameters.Add("@AppointmentDate", SqlDbType.Date).Value = appointmentDatePicker.SelectedDate;
                    cmd.Parameters.Add("@AppointmentTime", SqlDbType.Time, 7).Value = time;
                    cmd.Parameters.Add("@AppointmentCause", SqlDbType.VarChar, 256).Value = appointmentCauseTxt.Text;
                    cmd.Parameters.Add("@AppointmentStatus", SqlDbType.TinyInt).Value = addStatusParameter(status);

                    break;
                case "update":
                    msg = "Row Updated Successfully!";
                    cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = Int32.Parse(appointmentIDTxt.Text);
                    cmd.Parameters.Add("@PatientBirthNumber", SqlDbType.Char, 11).Value = patientBirthNumberTxt.SelectedItem;
                    cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = Int32.Parse(doctorIDTxt.SelectedItem.ToString());
                    cmd.Parameters.Add("@AppointmentDate", SqlDbType.Date).Value = appointmentDatePicker.SelectedDate;
                    cmd.Parameters.Add("@AppointmentTime", SqlDbType.Time, 7).Value = time;
                    cmd.Parameters.Add("@AppointmentCause", SqlDbType.VarChar, 256).Value = appointmentCauseTxt.Text;
                    cmd.Parameters.Add("@AppointmentStatus", SqlDbType.TinyInt).Value = addStatusParameter(status);

                    break;
                case "delete":
                    msg = "Row Deleted Successfully!";

                    cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = Int32.Parse(appointmentIDTxt.Text);

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
            String sql = "INSERT INTO " + table +
                            " (PatientBirthNumber, DoctorID, AppointmentDate, AppointmentTime, AppointmentCause, AppointmentStatus) " +
                            "VALUES(@PatientBirthNumber, @DoctorID, @AppointmentDate, @AppointmentTime, @AppointmentCause, @AppointmentStatus);";
            this.AUD(sql, "add");
        }

        private void updateBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "UPDATE " + table + " SET " +
                            "PatientBirthNumber=@PatientBirthNumber, DoctorID=@DoctorID, AppointmentDate=@AppointmentDate, " +
                            "AppointmentTime=@AppointmentTime, AppointmentCause=@AppointmentCause, AppointmentStatus=@AppointmentStatus " +
                            "WHERE AppointmentID = @AppointmentID";
            this.AUD(sql, "update");
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                appointmentIDTxt.Text = dr["AppointmentID"].ToString();
                patientBirthNumberTxt.Text = dr["PatientBirthNumber"].ToString();
                doctorIDTxt.Text = dr["DoctorID"].ToString();
                appointmentDatePicker.SelectedDate = DateTime.Parse(dr["AppointmentDate"].ToString());

                string[] time = dr["AppointmentTime"].ToString().Split(':');
                appointmentTimeHourPicker.SelectedItem = time[0];
                appointmentTimeMinutesPicker.SelectedItem = time[1];
                
                appointmentCauseTxt.Text = dr["AppointmentCause"].ToString();
                appointmentStatusComboBox.SelectedItem = dr["AppointmentStatus"].ToString();

                setStatusComboBox(dr["AppointmentStatus"].ToString());

                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;

            }
        }

        private void deleteBtnClick(object sender, RoutedEventArgs e)
        {
            String sql = "DELETE FROM " + table +
                            " WHERE AppointmentID = @AppointmentID";
            this.AUD(sql, "delete");
            this.resetAll();
        }

        private void resetBtnClick(object sender, RoutedEventArgs e)
        {
            resetAll();
        }

        private void resetAll()
        {
            appointmentIDTxt.Text = "Auto assigned";
            patientBirthNumberTxt.Text = "";
            doctorIDTxt.Text = "";
            appointmentDatePicker.SelectedDate= null;
            appointmentTimeHourPicker.SelectedItem = null;
            appointmentTimeMinutesPicker.SelectedItem = null;
            appointmentCauseTxt.Text = "";
            appointmentStatusComboBox.SelectedItem = null;

            addBtn.IsEnabled = true;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }



        private void loadTimeComboBoxes()
        {
            int hour;
            for(int i = 0; i < 9; i++)
            {
                hour = 8 + i;
                if (i < 2) 
                { appointmentTimeHourPicker.Items.Add("0"+hour.ToString()); 
                } else
                {
                    appointmentTimeHourPicker.Items.Add(hour.ToString());
                }
            }

            appointmentTimeMinutesPicker.Items.Add("00");
            appointmentTimeMinutesPicker.Items.Add("30");
        }

        private void loadStatusComboBox()
        {
            appointmentStatusComboBox.Items.Add("Not Approved");
            appointmentStatusComboBox.Items.Add("Approved");
        }

        private object addStatusParameter(object status)
        {
            object statusValue = DBNull.Value;
            switch (status)
            {
                case "Approved":
                    statusValue = 1;
                    break;
                case "Not Approved":
                    statusValue = 0;
                    break;
                case null:
                    statusValue = DBNull.Value;
                    break;
            }
                   return statusValue;
        }

        private void setStatusComboBox(string status)
        {
            switch (status)
            {
                case "1":
                    appointmentStatusComboBox.SelectedIndex = 1;
                    break;
                case "0":
                    appointmentStatusComboBox.SelectedIndex = 0;
                    break;
                case null:
                    appointmentStatusComboBox.SelectedIndex = 0;
                    break;
            }
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
