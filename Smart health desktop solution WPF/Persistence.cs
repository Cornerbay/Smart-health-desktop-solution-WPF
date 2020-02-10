﻿using MySql.Data.MySqlClient;
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

        private void executeAddQuery(string querySentence)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = querySentence;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        internal DataTable executeReadQuery(string querySentence)
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
        internal DataTable getSchema()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            connection.Open();
            DataTable dt = connection.GetSchema("Tables");
            connection.Close();

            return dt;
        }


        internal DataTable readTable(string table)
        {
        MySqlConnection connection = new MySqlConnection(connectionString);

        MySqlCommand cmd = new MySqlCommand("select*from "+table, connection);
        connection.Open();
        DataTable dt = new DataTable();
        dt.Load(cmd.ExecuteReader());
        connection.Close();

      
            return dt;
        }

        internal DataTable getColumnNames(string table)
        {
            string querySentence = "SHOW COLUMNS FROM " + table + ";";
            return executeReadQuery(querySentence);
        }

        internal void addPatient(Hashtable hashTable)
        {
            //MySqlCommand cmd = new MySqlCommand("insert into "+htPatientRow["Table"]+" values ("+ htPatientRow["BirthNumber"]+",'"+ htPatientRow["FirstName"]+"','"+ htPatientRow["LastName"]+"','"+ htPatientRow["Adress"]+"','"+ htPatientRow["Birthdate"]+"',"+ htPatientRow["AuthorizationLevel"]+" );", connection);
            string querySentence = "insert into " + hashTable["Table"] + " values (" + hashTable["BirthNumber"] + ",'" + hashTable["FirstName"] + "','" + hashTable["LastName"] + "','" + hashTable["Adress"] + "','" + hashTable["Birthdate"] + "'," + hashTable["AuthorizationLevel"] + " );";
            executeAddQuery(querySentence);
        }

        internal void addDoctor(Hashtable hashTable)
        {
            string querySentence = "insert into";
            executeAddQuery(querySentence);
        }

        internal void addMedicalHistory(Hashtable hashTable)
        {
            string querySentence = "insert into";
            executeAddQuery(querySentence);
        }

        internal void addLocation(Hashtable hashTable)
        {
            string querySentence = "insert into";
            executeAddQuery(querySentence);
        }

        internal void addTreatment(Hashtable hashTable)
        {
            string querySentence = "insert into";
            executeAddQuery(querySentence);
        }

        internal void addSpecialization(Hashtable hashTable)
        {
            string querySentence = "insert into";
            executeAddQuery(querySentence);
        }

        internal void addAppointment(Hashtable hashTable)
        {
            string querySentence = "insert into";
            executeAddQuery(querySentence);
        }

        internal void addCommunication(Hashtable hashTable)
        {
            string querySentence = "insert into";
            executeAddQuery(querySentence);
        }
    }
}
