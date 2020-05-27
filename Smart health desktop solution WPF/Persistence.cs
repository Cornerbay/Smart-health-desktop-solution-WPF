
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
                string[] array = { row[column].ToString(), row[column2].ToString() };
                comboBox.Items.Add(array);
            }
        }

        internal void setComboBox(ComboBox comboBox, string table, int column, int column2, int column3)
        {
            DataTable tableData = ReadTable(table);

            foreach (DataRow row in tableData.Rows)
            {
                comboBox.Items.Add(row[column]);
            }
        }

    }
}



