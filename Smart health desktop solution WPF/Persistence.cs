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

        static string connectionString = "SERVER=smart-health-solution.mysql.database.azure.com;DATABASE=app2000;UID=designerkaktus@smart-health-solution;PASSWORD=Vieralleveldigsunne1;";

        public object readTable(string table)
        {
        MySqlConnection connection = new MySqlConnection(connectionString);

        MySqlCommand cmd = new MySqlCommand("select*from "+table, connection);
        connection.Open();
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        connection.Close();

      
            return dt;
        }

        public void addPatient(Hashtable htPatientRow)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand cmd = new MySqlCommand("insert into "+htPatientRow["Table"]+" values ("+ htPatientRow["BirthNumber"]+",'"+ htPatientRow["FirstName"]+"','"+ htPatientRow["LastName"]+"','"+ htPatientRow["Adress"]+"','"+ htPatientRow["Birthdate"]+"',"+ htPatientRow["AuthorizationLevel"]+" );", connection);
          

            connection.Open();

            cmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
