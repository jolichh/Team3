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
    [ApiController]
    [Route("api/[controller]")]
    public class OfficeController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;

        public OfficeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn").ToString();
        }

        [HttpGet]
        [Route("GetAllOffices")]
        public string GetAllOffices()
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string query = "SELECT * FROM offices";
            DataTable queryResults = DAL.Query(query, connection);

            List<IModel> officeList = new();
            if (queryResults.Rows.Count > 0)
            {
                for (int i = 0; i < queryResults.Rows.Count; i++)
                {
                    Office office = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[i]["id"]),
                        Name = Convert.ToString(queryResults.Rows[i]["name"]),
                        CityId = Convert.ToInt32(queryResults.Rows[i]["office_id"])
                    };
                    officeList.Add(office);
                }
            }

            if (officeList.Count > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(officeList, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetOfficeByName/{name}")]
        public string GetOfficeByName(string name)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM offices WHERE name = '" + name + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                Office office = new() 
                {
                    Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                    Name = Convert.ToString(queryResults.Rows[0]["name"]),
                    CityId = Convert.ToInt32(queryResults.Rows[0]["city_id"])
                };
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(office, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetOfficeById/{id}")]
        public string GetOfficeById(int id)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM offices WHERE id = '" + id + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                Office office = new() 
                {
                    Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                    Name = Convert.ToString(queryResults.Rows[0]["name"]),
                    CityId = Convert.ToInt32(queryResults.Rows[0]["city_id"])
                };
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(office, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpPost]
        [Route("AddOffice")]
        public string AddOffice(SqlConnection connection, Office office)
        {
            SqlCommand command = new("INSERT INTO offices (name) VALUES ('"+office.Name+"')", connection);
            connection.Open();
            int commandStatus = command.ExecuteNonQuery();
            connection.Close();

            if (commandStatus > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(null, 201));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpPut]
        [Route("UpdateOffice")]
        public string UpdateOffice(SqlConnection connection, Office office)
        {
            SqlCommand command = new("UPDATE offices SET name = '"+office.Name+"' WHERE id = '"+office.Id+"'", connection);
            connection.Open();
            int commandStatus = command.ExecuteNonQuery();
            connection.Close();

            if (commandStatus > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(null, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpDelete]
        [Route("DeleteOffice/{id}")]
        public string DeleteOffice(SqlConnection connection, int id)
        {
            SqlCommand command = new("DELETE FROM offices WHERE id = '"+id+"'", connection);
            connection.Open();
            int commandStatus = command.ExecuteNonQuery();
            connection.Close();

            if (commandStatus > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(null, 204));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }
    }
}