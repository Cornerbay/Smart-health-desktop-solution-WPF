using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Smart_health_desktop_solution_WPF
{
    class Persistence
    {

        private static readonly string connectionString = "SERVER=smart-health-solution.mysql.database.azure.com;DATABASE=app2000;UID=designerkaktus@smart-health-solution;PASSWORD=Vieralleveldigsunne1;";

        private void ExecuteAddQuery(string querySentence)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = querySentence;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        internal DataTable ExecuteReadQuery(string querySentence)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
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
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            connection.Open();
            DataTable dt = connection.GetSchema("Tables");
            connection.Close();

            return dt;
        }


        internal DataTable ReadTable(string table)
        {
        MySqlConnection connection = new MySqlConnection(connectionString);

        MySqlCommand cmd = new MySqlCommand("select*from "+table, connection);
        connection.Open();
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        connection.Close();

      
            return dt;
        }

        internal DataTable GetColumnNames(string table)
        {
            string querySentence = "SHOW COLUMNS FROM " + table + ";";
            return ExecuteReadQuery(querySentence);
        }

        internal void AddPatient(Hashtable hashTable)
        {
            //MySqlCommand cmd = new MySqlCommand("insert into "+htPatientRow["Table"]+" values ("+ htPatientRow["BirthNumber"]+",'"+ htPatientRow["FirstName"]+"','"+ htPatientRow["LastName"]+"','"+ htPatientRow["Adress"]+"','"+ htPatientRow["Birthdate"]+"',"+ htPatientRow["AuthorizationLevel"]+" );", connection);
            string querySentence = "insert into " + hashTable["Table"] + " values (" + hashTable["BirthNumber"] + ",'" + hashTable["FirstName"] + "','" + hashTable["LastName"] + "','" + hashTable["Adress"] + "','" + hashTable["Birthdate"] + "'," + hashTable["AuthorizationLevel"] + " );";
            ExecuteAddQuery(querySentence);
        }

        internal void AddQuerySentence(Hashtable hashTable)
        {
            string querySentence = "insert into ";
            foreach (DictionaryEntry entry in hashTable)
            {
                if(entry.Key == "Table")
                {
                    querySentence += entry.Value + " values (";
                }
                else if((entry.Value is short || entry.Value is int || entry.Value is long))
                {
                    querySentence += entry.Value + ",";
                }
                else
                {

                querySentence += "'" + entry.Value + "',";
                }    
            };
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
