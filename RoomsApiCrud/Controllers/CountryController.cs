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
    public class CountryController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;

        public CountryController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn").ToString();
        }

        [HttpGet]
        [Route("GetAllCountries")]
        public string GetAllCountries()
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string query = "SELECT * FROM countries";
            DataTable queryResults = DAL.Query(query, connection);

            List<Country> countryList = new();
            if (queryResults.Rows.Count > 0)
            {
                for (int i = 0; i < queryResults.Rows.Count; i++)
                {
                    Country country = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[i]["_id"]),
                        Name = Convert.ToString(queryResults.Rows[i]["_name"])
                    };
                    countryList.Add(country);
                }
            }

            if (countryList.Count > 0)
            {
                return JsonConvert.SerializeObject(countryList);
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetCountryById/{name}")]
        public string GetCountryByName(string name)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM countries WHERE _name = '" + name + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            Country country = new();
            if (queryResults.Rows.Count > 0)
            {
                country.Id = Convert.ToInt32(queryResults.Rows[0]["_id"]);
                country.Name = Convert.ToString(queryResults.Rows[0]["_name"]);

                return JsonConvert.SerializeObject(country);
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpPost]
        [Route("AddCountry")]
        public string AddCountry(SqlConnection connection, Country country)
        {
            SqlCommand command = new("INSERT INTO Countries (_name) VALUES ('"+country.Name+"')", connection);
            connection.Open();
            int commandStatus = command.ExecuteNonQuery();
            connection.Close();

            Response<IModel?> response = new();
            if (commandStatus > 0)
            {
                response.StatusCode = 201;
                response.StatusMessage = "Success.";
                response.Result = null;
                return JsonConvert.SerializeObject(response);
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }
    }

}