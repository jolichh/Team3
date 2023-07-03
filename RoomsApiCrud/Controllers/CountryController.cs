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
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn")!;
        }

        [HttpGet]
        [Route("GetAllCountries")]
        public ActionResult GetAllCountries()
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string query = "SELECT * FROM dbo.countries";
            DataTable queryResults = DAL.Query(query, connection);

            List<IModel> countryList = new();
            if (queryResults.Rows.Count > 0)
            {
                for (int i = 0; i < queryResults.Rows.Count; i++)
                {
                    Country country = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[i]["id"]),
                        Name = Convert.ToString(queryResults.Rows[i]["name"])
                    };
                    countryList.Add(country);
                }
            }

            if (countryList.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(countryList));
                //return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(countryList, 200));
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetCountryByName/{name}")]
        public ActionResult GetCountryByName(string name)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM countries WHERE name = '" + name + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                Country country = new()
                {
                    Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                    Name = Convert.ToString(queryResults.Rows[0]["name"])
                };
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(country));
                //return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(country, 200));
            }

            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        [Route("GetCountryById/{id}")]
        public ActionResult GetCountryById(int id)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM countries WHERE id = '" + id + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                Country country = new()
                {
                    Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                    Name = Convert.ToString(queryResults.Rows[0]["name"])
                };
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(country));
                //return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(country, 200));
            }

            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [Route("AddCountry")]
        public ActionResult AddCountry(Country country)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("INSERT INTO countries (name) VALUES ('"+country.Name+"')", connection);
            int commandStatus = DAL.Command(command, connection);

            if (commandStatus > 0)
            {
                return StatusCode(StatusCodes.Status201Created, null);
                //return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(null, 201));
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpPut]
        [Route("UpdateCountry")]
        public ActionResult UpdateCountry(Country country)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("UPDATE countries SET name = '"+country.Name+"' WHERE id = '"+country.Id+"'", connection);
            int commandStatus = DAL.Command(command, connection);

            if (commandStatus > 0)
            {
                return StatusCode(StatusCodes.Status200OK, null);
                //return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(null, 200));
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpDelete]
        [Route("DeleteCountry/{id}")]
        public ActionResult DeleteCountry(int id)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("DELETE FROM countries WHERE id = '"+id+"'", connection);
            int commandStatus = DAL.Command(command, connection);

            if (commandStatus > 0)
            {
                //return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(null, 204));
                return StatusCode(204, null);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }
    }
}