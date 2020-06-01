
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Smart_health_desktop_solution_WPF
{
    class Persistence
    {

        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con = null;

        private void setConnection()
        {
            con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception exp)
            {
            }

        }

        internal DataTable ExecuteReadQuery(string querySentence)
        {
            SqlCommand cmd = new SqlCommand();
            setConnection();
            cmd.Connection = con;
            cmd.CommandText = querySentence;
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            return dt;
        }

        internal DataTable ReadTable(string table)
        {
            setConnection();
            SqlCommand cmd = new SqlCommand("select*from " + table, con);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();

            return dt;
        }

        internal DataTable ReadMyAppointmentsTable(string table, string userID)
        {
            setConnection();
            SqlCommand cmd = new SqlCommand("select*from " + table + " WHERE DoctorID = " + Int32.Parse(userID) +";", con);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();

            return dt;
        }

        internal DataTable SearchMyAppointmentsTable(string table, string column, string search, string userID)
        {
            setConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            search = "%" + search + "%";
            cmd.Parameters.Add("@Search", SqlDbType.VarChar).Value = search;
            cmd.CommandText = "select * from " + table + " where " + column + " LIKE @Search AND DoctorID = " + Int32.Parse(userID) + ";";

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();

            return dt;
        }

        internal DataTable SearchTable(string table, string column, string search)
        {
            setConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            search = "%" + search + "%";
            cmd.Parameters.Add("@Search", SqlDbType.VarChar).Value = search;
            cmd.CommandText = "select * from "+ table + " where " + column + " LIKE @Search ;";

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();

            return dt;
        }

        private Object[] FindTypeAndLength(string table, string column)
        {
            string querySentence = "SELECT DATA_TYPE, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '"+
                                    table +"' AND COLUMN_NAME = '" + column + "';";
            DataRow dr = ExecuteReadQuery(querySentence).Rows[0];
            object[] typeAndLength = { dr["DATA_TYPE"], dr["CHARACTER_MAXIMUM_LENGTH"] };
            return typeAndLength;
        }

        internal DataTable GetSchema()
        {
            SqlCommand cmd = new SqlCommand();
            setConnection();
            cmd.Connection = con;
            DataTable dt = con.GetSchema("Tables");
            con.Close();

            return dt;
        }

        internal DataTable GetColumnNames(string table)
        {
            string querySentence = "select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + table + "';";
            return ExecuteReadQuery(querySentence);
        }
        internal void setComboBox(ComboBox comboBox, DataTable tableData, int column)
        {
            foreach (DataRow row in tableData.Rows)
            {
                comboBox.Items.Add(row[column]);
            }
        }

        internal void setComboBox(ComboBox comboBox, string table, int column)
        {
            DataTable tableData = ReadTable(table);

            foreach (DataRow row in tableData.Rows)
            {
                comboBox.Items.Add(row[column]);
            }
        }


        internal void setComboBox(ComboBox comboBox, string table, int column, int column2)
        {
            DataTable tableData = ReadTable(table);

            foreach (DataRow row in tableData.Rows)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = row[column2].ToString();
                item.Value = row[column].ToString();

                comboBox.Items.Add(item);
            }
        }

        internal void setComboBox(ComboBox comboBox, string table, int column, int column2, int column3)
        {
            DataTable tableData = ReadTable(table);

            foreach (DataRow row in tableData.Rows)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = row[column2].ToString() + " " + row[column3];
                item.Value = row[column].ToString();

                comboBox.Items.Add(item);
            }
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return Text + "\n" + "Identifier: " + Value;
            }
        }

        public string getHash(string username)
        {
            var hash = "";

            DataTable adminDataTable = ReadTable("Admin");
            DataTable doctorDataTable = ReadTable("Doctor");

            foreach (DataRow row in adminDataTable.Rows)
            {
                if (username.Equals(row[0].ToString()))
                {
                    hash = row[3].ToString();
                    return hash;
                }
            }

            foreach (DataRow row in doctorDataTable.Rows)
            {
                if (username.Equals(row[0].ToString()))
                {
                    hash = row[5].ToString();
                    return hash;
                }
            }

            return hash;
        }

        public String[] GetUser(string username, string table)
        {
            String[] user = new String[4];

            switch (table)
            {
                case "Admin":
                    DataTable adminDataTable = ReadTable("Admin");
                    foreach (DataRow row in adminDataTable.Rows)
                    {
                        if (username.Equals(row[0].ToString()))
                        {
                            user[0] = "1";
                            user[1] = (row[3].ToString()); //adds password hash
                            user[2] = (row[1].ToString()); //adds first name
                            user[3] = (row[2].ToString()); // adds last name
                            return user;
                        }
                    }
                    break;
                case "Doctor":
                    DataTable doctorDataTable = ReadTable("Doctor");
                    foreach (DataRow row in doctorDataTable.Rows)
                    {
                        if (username.Equals(row[0].ToString()))
                        {
                            user[0] = "1";
                            user[1] = (row[5].ToString()); //adds password hash
                            user[2] = (row[2].ToString()); //adds first name
                            user[3] = (row[3].ToString()); //adds last name
                            return user;
                        }
                    }
                    break;
            }

            return user;
        }

    }
}



