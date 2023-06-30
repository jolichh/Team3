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
    public class RoomController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;

        public RoomController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn")!;
        }

        [HttpGet]
        [Route("GetAllRooms")]
        public string GetAllRooms()
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string query = "SELECT * FROM dbo.rooms";
            DataTable queryResults = DAL.Query(query, connection);

            List<IModel> roomList = new();
            if (queryResults.Rows.Count > 0)
            {
                for (int i = 0; i < queryResults.Rows.Count; i++)
                {
                    Room room = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[i]["id"]),
                        Name = Convert.ToString(queryResults.Rows[i]["name"]),
                        OfficeId = Convert.ToInt32(queryResults.Rows[i]["office_id"])
                    };
                    roomList.Add(room);
                }
            }

            if (roomList.Count > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(roomList, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetAllRoomsByOfficeId/{officeId}")]
        public string GetAllRoomsByOfficeId(int officeId)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string query = "SELECT * FROM rooms JOIN offices ON rooms.office_id = offices.id AND offices.id = '"+officeId+"'";
            DataTable queryResults = DAL.Query(query, connection);

            List<IModel> roomList = new();
            if (queryResults.Rows.Count > 0)
            {
                for (int i = 0; i < queryResults.Rows.Count; i++)
                {
                    Room room = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[i]["id"]),
                        Name = Convert.ToString(queryResults.Rows[i]["name"]),
                        OfficeId = Convert.ToInt32(queryResults.Rows[i]["office_id"])
                    };
                    roomList.Add(room);
                }
            }

            if (roomList.Count > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(roomList, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetAllRoomsByCityId/{cityId}")]
        public string GetAllRoomsByCityId(int cityId)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string query = "SELECT * FROM rooms JOIN offices ON rooms.office_id = offices.id JOIN cities ON offices.city_id = cities.id AND cities.id = '" + cityId + "'";
            DataTable queryResults = DAL.Query(query, connection);

            List<IModel> roomList = new();
            if (queryResults.Rows.Count > 0)
            {
                for (int i = 0; i < queryResults.Rows.Count; i++)
                {
                    Room room = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[i]["id"]),
                        Name = Convert.ToString(queryResults.Rows[i]["name"]),
                        OfficeId = Convert.ToInt32(queryResults.Rows[i]["office_id"])
                    };
                    roomList.Add(room);
                }
            }

            if (roomList.Count > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(roomList, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetRoomByName/{name}")]
        public string GetRoomByName(string name)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM rooms WHERE name = '" + name + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                Room room = new() 
                {
                    Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                    Name = Convert.ToString(queryResults.Rows[0]["name"]),
                    OfficeId = Convert.ToInt32(queryResults.Rows[0]["office_id"])
                };
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(room, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetRoomById/{id}")]
        public string GetRoomById(int id)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM room WHERE id = '" + id + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                Room room = new() 
                {
                    Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                    Name = Convert.ToString(queryResults.Rows[0]["name"]),
                    OfficeId = Convert.ToInt32(queryResults.Rows[0]["office_id"])
                };
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(room, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpPost]
        [Route("AddRoom")]
        public string AddRoom(Room room)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("INSERT INTO rooms (name, office_id) VALUES ('"+room.Name+"', '"+room.OfficeId+"')", connection);
            int commandStatus = DAL.Command(command, connection);

            if (commandStatus > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(null, 201));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpPut]
        [Route("UpdateRoom")]
        public string UpdateRoom(Room room)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("UPDATE room SET name = '"+room.Name+"', office_id = '"+room.OfficeId+"' WHERE id = '"+room.Id+"'", connection);
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
        [Route("DeleteRoom/{id}")]
        public string DeleteRoom(int id)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("DELETE FROM rooms WHERE id = '"+id+"'", connection);
            int commandStatus = DAL.Command(command, connection);

            if (commandStatus > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(null, 204));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }
    }
}