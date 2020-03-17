
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Smart_health_desktop_solution_WPF
{
    class Persistence
    {

        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private void ExecuteAddQuery(string querySentence)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = querySentence;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        internal DataTable ExecuteReadQuery(string querySentence)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = querySentence;
            DataTable dt = new DataTable();
            connection.Open();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            return dt;
        }
        internal DataTable GetSchema()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            connection.Open();
            DataTable dt = connection.GetSchema("Tables");
            connection.Close();

            return dt;
        }


        internal DataTable ReadTable(string table)
        {
        SqlConnection connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand("select*from "+table, connection);
        connection.Open();
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        connection.Close();

      
            return dt;
        }

        internal DataTable GetColumnNames(string table)
        {
            string querySentence = "select COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + table + "';";
            /*string querySentence = "SELECT name user_type_id FROM sys.columns WHERE OBJECT_ID = OBJECT_ID('" + table + "');";*/
            /*string querySentence = "SHOW COLUMNS FROM " + table + ";";*/
            return ExecuteReadQuery(querySentence);
        }

        internal void AddPatient(Hashtable hashTable)
        {
            //SqlCommand cmd = new SqlCommand("insert into "+htPatientRow["Table"]+" values ("+ htPatientRow["BirthNumber"]+",'"+ htPatientRow["FirstName"]+"','"+ htPatientRow["LastName"]+"','"+ htPatientRow["Adress"]+"','"+ htPatientRow["Birthdate"]+"',"+ htPatientRow["AuthorizationLevel"]+" );", connection);
            string querySentence = "insert into " + hashTable["Table"] + " values (" + hashTable["BirthNumber"] + ",'" + hashTable["FirstName"] + "','" + hashTable["LastName"] + "','" + hashTable["Adress"] + "','" + hashTable["Birthdate"] + "'," + hashTable["AuthorizationLevel"] + " );";
            ExecuteAddQuery(querySentence);
        }

        internal void AddQuerySentence(ArrayList myList, String tableName)
        {
            Regex regex = new Regex("[^0-9]+");
            string querySentence = "insert into " + tableName + " values (";
            for(int i = 0; i < myList.Count; i++)
            {
                if(!string.IsNullOrEmpty((string)myList[i]))
                {
                    if (regex.IsMatch(myList[i].ToString()))
                    {
                        querySentence += " '" + myList[i] + "',";
                    }
                    else
                    {
                        querySentence += " " + myList[i] + ",";
                    }
                }
            }
            querySentence = querySentence.Remove(querySentence.Length - 1);
            querySentence += ");";

            ExecuteAddQuery(querySentence);
        }

        internal void AddDoctor(Hashtable hashTable)
        {
            string querySentence = "insert into";
            ExecuteAddQuery(querySentence);
        }

        internal void AddMedicalHistory(Hashtable hashTable)
        {
            string querySentence = "insert into";
            ExecuteAddQuery(querySentence);
        }

        internal void AddLocation(Hashtable hashTable)
        {
            string querySentence = "insert into";
            ExecuteAddQuery(querySentence);
        }

        internal void AddTreatment(Hashtable hashTable)
        {
            string querySentence = "insert into";
            ExecuteAddQuery(querySentence);
        }

        internal void AddSpecialization(Hashtable hashTable)
        {
            string querySentence = "insert into";
            ExecuteAddQuery(querySentence);
        }

        internal void addAppointment(Hashtable hashTable)
        {
            string querySentence = "insert into";
            ExecuteAddQuery(querySentence);
        }

        internal void addCommunication(Hashtable hashTable)
        {
            string querySentence = "insert into";
            ExecuteAddQuery(querySentence);
        }
    }
}
