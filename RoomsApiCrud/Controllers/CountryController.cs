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

        public CountryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllCountries")]
        public string GetAllCountries(SqlConnection connection)
        {
            //SqlConnection connection = new(_configuration.GetConnectionString("RoomsApiCrudConn").ToString());
            
            SqlDataAdapter dataAdapter = new("SELECT * FROM countries", connection);
            DataTable dataTable = new();
            dataAdapter.Fill(dataTable);
            List<Country> countryList = new();
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Country country = new();
                    country.Id = Convert.ToInt32(dataTable.Rows[i]["_id"]);
                    country.Name = Convert.ToString(dataTable.Rows[i]["_name"]);
                    countryList.Add(country);
                }
            }

            if (countryList.Count > 0)
            {
                return JsonConvert.SerializeObject(countryList);
            }
            
            CountryResponse response = new();
            response.StatusCode = 500;
            response.StatusMessage = "Operation failed.";
            return JsonConvert.SerializeObject(response);
        }

        [HttpGet]
        [Route("GetCountryById/{id}")]
        public string GetCountryById(SqlConnection connection, int id)
        {
            //SqlConnection connection = new(_configuration.GetConnectionString("RoomsApiCrudConn").ToString());
            
            SqlDataAdapter dataAdapter = new("SELECT * FROM countries WHERE _id = '"+id+"'", connection);
            DataTable dataTable = new();
            dataAdapter.Fill(dataTable);
            Country country = new();
            if (dataTable.Rows.Count > 0)
            {
                country.Id = Convert.ToInt32(dataTable.Rows[0]["_id"]);
                country.Name = Convert.ToString(dataTable.Rows[0]["_name"]);
            }

            if (country.Name != null)
            {
                return JsonConvert.SerializeObject(country);
            }
            
            CountryResponse response = new();
            response.StatusCode = 500;
            response.StatusMessage = "Operation failed.";
            return JsonConvert.SerializeObject(response);
        }

        [HttpPost]
        [Route("AddCountry")]
        public string AddCountry(SqlConnection connection, Country country)
        {
            SqlCommand command = new("INSERT INTO Countries (_name) VALUES ('"+country.Name+"')", connection);
            connection.Open();
            int commandStatus = command.ExecuteNonQuery();
            connection.Close();

            CountryResponse response = new();
            if (commandStatus > 0)
            {
                response.StatusCode = 201;
                response.StatusMessage = "Success.";
                return JsonConvert.SerializeObject(response);
            }

            response.StatusCode = 500;
            response.StatusMessage = "Operation failed.";
            return JsonConvert.SerializeObject(response);
        }
    }

}