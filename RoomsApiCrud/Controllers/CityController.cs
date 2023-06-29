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
    public class CityController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;

        public CityController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn").ToString();
        }

        [HttpGet]
        [Route("GetAllCities")]
        public string GetAllCities()
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string query = "SELECT * FROM cities";
            DataTable queryResults = DAL.Query(query, connection);

            List<IModel> cityList = new();
            if (queryResults.Rows.Count > 0)
            {
                for (int i = 0; i < queryResults.Rows.Count; i++)
                {
                    City city = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[i]["id"]),
                        Name = Convert.ToString(queryResults.Rows[i]["name"]),
                        CountryId = Convert.ToInt32(queryResults.Rows[i]["country_id"])
                    };
                    cityList.Add(city);
                }
            }

            if (cityList.Count > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(cityList, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetAllCitiesByCountry/{country}")]
        public string GetAllCities(Country country)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string query = "SELECT * FROM cities WHERE country_id = '" + country.Id + "'";
            DataTable queryResults = DAL.Query(query, connection);

            List<IModel> cityList = new();
            if (queryResults.Rows.Count > 0)
            {
                for (int i = 0; i < queryResults.Rows.Count; i++)
                {
                    City city = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[i]["id"]),
                        Name = Convert.ToString(queryResults.Rows[i]["name"]),
                        CountryId = Convert.ToInt32(queryResults.Rows[i]["country_id"])
                    };
                    cityList.Add(city);
                }
            }

            if (cityList.Count > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(cityList, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetCityByName/{name}")]
        public string GetCityByName(string name)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM cities WHERE name = '" + name + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                City city = new() 
                {
                    Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                    Name = Convert.ToString(queryResults.Rows[0]["name"]),
                    CountryId = Convert.ToInt32(queryResults.Rows[0]["country_id"])
                };
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(city, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetCityById/{id}")]
        public string GetCityById(int id)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM cities WHERE id = '" + id + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                City city = new() 
                {
                    Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                    Name = Convert.ToString(queryResults.Rows[0]["name"]),
                    CountryId = Convert.ToInt32(queryResults.Rows[0]["country_id"])
                };
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(city, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpPost]
        [Route("AddCity")]
        public string AddCity(City city)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("INSERT INTO cities (name) VALUES ('"+city.Name+"', '"+city.CountryId+"')", connection);
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
        [Route("UpdateCity")]
        public string UpdateCity(City city)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("UPDATE cities SET name = '"+city.Name+"', country_id = '"+city.CountryId+"' WHERE id = '"+city.Id+"'", connection);
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
        [Route("DeleteCity/{id}")]
        public string DeleteCity(int id)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("DELETE FROM cities WHERE id = '"+id+"'", connection);
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