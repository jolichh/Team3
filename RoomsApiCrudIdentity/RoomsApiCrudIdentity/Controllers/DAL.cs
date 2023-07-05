using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using RoomsApiCrud.Models;

namespace RoomsApiCrud.Controllers
{
    public class DAL
    {
        public static SqlConnection Connect(string ConnectionString)
        {
            return new(ConnectionString);
        }

        public static DataTable Query(string queryString, SqlConnection connection)
        {
            SqlDataAdapter dataAdapter = new(queryString, connection);
            DataTable dataTable = new();
            dataAdapter.Fill(dataTable);
            return dataTable;
        }

        public static int Command(SqlCommand command, SqlConnection connection)
        {
            //SqlCommand command = new(command, connection);
            connection.Open();
            int statusValue = command.ExecuteNonQuery();
            connection.Close();
            return statusValue;
        }
    }
}