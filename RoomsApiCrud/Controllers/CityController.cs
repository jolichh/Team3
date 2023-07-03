using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
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
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn")!;
        }

        [HttpGet]
        [Route("GetAllCities")]
        public ActionResult GetAllCities()
        {
            string query = "SELECT * FROM dbo.cities";
            SqlConnection connection = DAL.Connect(_connectionString);
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
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(cityList));
                //return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(cityList, 200));
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetAllCitiesByCountryId/{countryId}")]
       
        public ActionResult GetAllCitiesByCountry(int countryId)
        {
            string query = "SELECT * FROM cities WHERE country_id = '" +countryId+ "'";
            SqlConnection connection = DAL.Connect(_connectionString);
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
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(cityList));
                //return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(cityList, 200));
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetCityByName/{name}")]
        public ActionResult GetCityByName(string name)
        {
            string queryString = "SELECT * FROM cities WHERE name = '" + name + "'";
            SqlConnection connection = DAL.Connect(_connectionString);
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                City city = new() 
                {
                    Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                    Name = Convert.ToString(queryResults.Rows[0]["name"]),
                    CountryId = Convert.ToInt32(queryResults.Rows[0]["country_id"])
                };
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(city));
                //return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(city, 200));
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetCityById/{id}")]
        public ActionResult GetCityById(int id)
        {
            string queryString = "SELECT * FROM cities WHERE id = '" + id + "'";
            SqlConnection connection = DAL.Connect(_connectionString);
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                City city = new() 
                {
                    Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                    Name = Convert.ToString(queryResults.Rows[0]["name"]),
                    CountryId = Convert.ToInt32(queryResults.Rows[0]["country_id"])
                };
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(city));
                //return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(city, 200));
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpPost]
        [Route("AddCity")]
        public ActionResult AddCity(City city)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("INSERT INTO cities (name, country_id) VALUES ('"+city.Name+"', '"+city.CountryId+"')", connection);
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
        [Route("UpdateCity")]
        public ActionResult UpdateCity(City city)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("UPDATE cities SET name = '"+city.Name+"', country_id = '"+city.CountryId+"' WHERE id = '"+city.Id+"'", connection);
            int commandStatus = DAL.Command(command, connection);

            if (commandStatus > 0)
            {
            return StatusCode(StatusCodes.Status200OK, null);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpDelete]
        [Route("DeleteCity/{id}")]
        public ActionResult DeleteCity(int id)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("DELETE FROM cities WHERE id = '"+id+"'", connection);
            int commandStatus = DAL.Command(command, connection);

            if (commandStatus > 0)
            {
                return StatusCode(StatusCodes.Status204NoContent, null);
                //return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(null, 204));
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
            //return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }
    }
}