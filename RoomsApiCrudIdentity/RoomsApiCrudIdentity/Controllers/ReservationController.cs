using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using System.Data;

using RoomsApiCrudIdentity.Data;
using RoomsApiCrudIdentity.Entities;

namespace RoomsApiCrudIdentity.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        private readonly RoomsApiCrudDbContext _context;

        public ReservationController(IConfiguration configuration, RoomsApiCrudDbContext context)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn")!;
            _context = context;
        }

        [HttpGet]
        [Route("GetAllReservations")]
        public async Task<IActionResult> GetAllReservations()
        {
            return Ok(await _context.Reservations.ToListAsync());
        }

        [HttpGet]
        [Route("GetReservationById")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            return Ok(await _context.Reservations.FindAsync(id));
        }

        [HttpGet]
        [Route("GetReservationsByRoomId")]
        public async Task<IActionResult> GetReservationsByRoomId(int roomId)
        {
            var result = await _context.Reservations
                .Where(
                    reservation => reservation.RoomId == roomId)
                .ToListAsync();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetReservationsByOfficeId")]
        public async Task<IActionResult> GetReservationsByOfficeId(int officeId)
        {
            var result = await _context.Reservations.Join(
                    _context.Rooms,
                    reservation => reservation.RoomId,
                    room => room.Id,
                    (reservation, room) => new { Reservation = reservation, Room = room })
                .Join(
                    _context.Offices,
                    roomReservation => roomReservation.Room.OfficeId,
                    office => office.Id,
                    (roomReservation, office) => new { Room = roomReservation.Room, Office = office })
                .Where(
                    officeRoomReservation => officeRoomReservation.Room.OfficeId == officeId)
                .ToListAsync();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetReservationsByCityId")]
        public async Task<IActionResult> GetReservationsByCityId(int cityId)
        {
            var result = await _context.Reservations.Join(
                    _context.Rooms,
                    reservation => reservation.RoomId,
                    room => room.Id,
                    (reservation, room) => new { Reservation = reservation, Room = room })
                .Join(
                    _context.Offices,
                    roomReservation => roomReservation.Room.OfficeId,
                    office => office.Id,
                    (roomReservation, office) => new { roomReservation, Office = office })
                .Join(
                    _context.Cities,
                    officeRoomReservation => officeRoomReservation.Office.CityId,
                    city => city.Id,
                    (officeRoomReservation, city) => new { Office = officeRoomReservation.Office, City = city }
                )
                .Where(
                    cityOfficeRoomReservation => cityOfficeRoomReservation.Office.CityId == cityId)
                .ToListAsync();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetReservationsByCountryId")]
        public async Task<IActionResult> GetReservationsByCountryId(int countryId)
        {
            var result = await _context.Reservations.Join(
                    _context.Rooms,
                    reservation => reservation.RoomId,
                    room => room.Id,
                    (reservation, room) => new { Reservation = reservation, Room = room })
                .Join(
                    _context.Offices,
                    roomReservation => roomReservation.Room.OfficeId,
                    office => office.Id,
                    (roomReservation, office) => new { roomReservation, Office = office })
                .Join(
                    _context.Cities,
                    officeRoomReservation => officeRoomReservation.Office.CityId,
                    city => city.Id,
                    (officeRoomReservation, city) => new { officeRoomReservation, City = city }
                )
                .Join(
                    _context.Countries,
                    cityOfficeRoomReservation => cityOfficeRoomReservation.City.CountryId,
                    country => country.Id,
                    (cityOfficeRoomReservation, country) => new { City = cityOfficeRoomReservation.City, Country = country })
                .Where(
                    countryCityOfficeRoomReservation => countryCityOfficeRoomReservation.City.CountryId == countryId)
                .ToListAsync();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateReservation")]
        public async Task<IActionResult> CreateReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return Created($"/GetReservationById?id={reservation.Id}", reservation);
        }

        [HttpPut]
        [Route("UpdateReservation")]
        public async Task<IActionResult> UpdateReservation(Reservation reservationToUpdate)
        {
            _context.Reservations.Update(reservationToUpdate);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteReservation")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservationToDelete = await _context.Reservations.FindAsync(id);
            if (reservationToDelete == null)
            {
                return NotFound();
            }
            _context.Reservations.Remove(reservationToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}