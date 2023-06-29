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
    public class ReservationController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;

        public ReservationController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn").ToString();
        }

        [HttpGet]
        [Route("GetAllReservations")]
        public string GetAllReservations()
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string query = "SELECT * FROM reservations";
            DataTable queryResults = DAL.Query(query, connection);

            List<IModel> reservationList = new();
            if (queryResults.Rows.Count > 0)
            {
                for (int i = 0; i < queryResults.Rows.Count; i++)
                {
                    Reservation reservation = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[i]["id"]),
                        Date = Convert.ToDateTime(queryResults.Rows[i]["date"]),
                        StartTime = Convert.ToDateTime(queryResults.Rows[i]["start_time"]),
                        EndTime = Convert.ToDateTime(queryResults.Rows[i]["end_time"]),
                        RoomId = Convert.ToInt32(queryResults.Rows[i]["room_id"]),
                        UserId = Convert.ToInt32(queryResults.Rows[i]["user_id"])
                    };
                    reservationList.Add(reservation);
                }
            }

            if (reservationList.Count > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(reservationList, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetAllReservationsByOffice/{office}")]
        public string GetAllReservationsByOffice(Office office)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string query = "SELECT * FROM reservations WHERE office_id = '"+office.Id+"'";
            DataTable queryResults = DAL.Query(query, connection);

            List<IModel> reservationList = new();
            if (queryResults.Rows.Count > 0)
            {
                for (int i = 0; i < queryResults.Rows.Count; i++)
                {
                    Reservation reservation = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[i]["id"]),
                        Date = Convert.ToDateTime(queryResults.Rows[i]["date"]),
                        StartTime = Convert.ToDateTime(queryResults.Rows[i]["start_time"]),
                        EndTime = Convert.ToDateTime(queryResults.Rows[i]["end_time"]),
                        RoomId = Convert.ToInt32(queryResults.Rows[i]["room_id"]),
                        UserId = Convert.ToInt32(queryResults.Rows[i]["user_id"])
                    };
                    reservationList.Add(reservation);
                }
            }

            if (reservationList.Count > 0)
            {
                return JsonConvert.SerializeObject(ResponseFactory.CreateListResultSuccess(reservationList, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetReservationById/{id}")]
        public string GetReservationById(int id)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM reservations WHERE id = '" + id + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                Reservation reservation = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                        Date = Convert.ToDateTime(queryResults.Rows[0]["date"]),
                        StartTime = Convert.ToDateTime(queryResults.Rows[0]["start_time"]),
                        EndTime = Convert.ToDateTime(queryResults.Rows[0]["end_time"]),
                        RoomId = Convert.ToInt32(queryResults.Rows[0]["room_id"]),
                        UserId = Convert.ToInt32(queryResults.Rows[0]["user_id"])
                    };
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(reservation, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpGet]
        [Route("GetReservationByOfficeId/{office_id}")]
        public string GetReservationByOfficeId(string officeId)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            string queryString = "SELECT * FROM reservations WHERE office_id = '" + officeId + "'";
            DataTable queryResults = DAL.Query(queryString, connection);

            if (queryResults.Rows.Count > 0)
            {
                Reservation reservation = new()
                    {
                        Id = Convert.ToInt32(queryResults.Rows[0]["id"]),
                        Date = Convert.ToDateTime(queryResults.Rows[0]["date"]),
                        StartTime = Convert.ToDateTime(queryResults.Rows[0]["start_time"]),
                        EndTime = Convert.ToDateTime(queryResults.Rows[0]["end_time"]),
                        RoomId = Convert.ToInt32(queryResults.Rows[0]["room_id"]),
                        UserId = Convert.ToInt32(queryResults.Rows[0]["user_id"])
                    };
                return JsonConvert.SerializeObject(ResponseFactory.CreateSingleResultSuccess(reservation, 200));
            }

            return JsonConvert.SerializeObject(ResponseFactory.Create500());
        }

        [HttpPost]
        [Route("AddReservation")]
        public string AddReservation(Reservation reservation)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("INSERT INTO reservations VALUES ('"+reservation.Id+"', '"+reservation.Date+"', '"+reservation.StartTime+"', '"+reservation.EndTime+"', '"+reservation.RoomId+"', '"+reservation.UserId+"',)", connection);
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
        [Route("UpdateReservation")]
        public string UpdateReservation(Reservation reservation)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
            SqlCommand command = new("UPDATE offices SET id = '"+reservation.Id+"', date = '"+reservation.Date+"', start_time = '"+reservation.StartTime+"', end_time = '"+reservation.EndTime+"', room_id = '"+reservation.RoomId+"', user_id = '"+reservation.UserId+"' WHERE id = '"+reservation.Id+"'", connection);
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
        public string DeleteOffice(int id)
        {
            SqlConnection connection = DAL.Connect(_connectionString);
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