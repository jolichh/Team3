using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using System.Data;

using RoomsApiCrudIdentity.Data;
using RoomsApiCrudIdentity.Entities;
using RoomsApiCrudIdentity.Models;

namespace RoomsApiCrudIdentity.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        private readonly RoomsApiCrudDbContext _context;

        public RoomController(IConfiguration configuration, RoomsApiCrudDbContext context)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn")!;
            _context = context;
        }

        [HttpGet]
        [Route("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            return Ok(await _context.Rooms.ToListAsync());
        }

        [HttpGet]
        [Route("GetRoomById")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            return Ok(await _context.Rooms.FindAsync(id));
        }

        [HttpGet]
        [Route("GetRoomsByOfficeId")]
        public async Task<IActionResult> GetRoomsByOfficeId(int officeId)
        {
            var result = await _context.Rooms
                .Where(
                    room => room.OfficeId == officeId)
                .ToListAsync();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetRoomsByCityId")]
        public async Task<IActionResult> GetRoomsByCityId(int cityId)
        {
            var result = await _context.Rooms.Join(
                    _context.Offices,
                    room => room.OfficeId,
                    office => office.Id,
                    (room, office) => new { Room = room, Office = office })
                .Join(
                    _context.Cities,
                    officeRoom => officeRoom.Office.CityId,
                    city => city.Id,
                    (officeRoom, city) => new { Office = officeRoom.Office, City = city })
                .Where(
                    cityOfficeRoom => cityOfficeRoom.Office.CityId == cityId)
                .ToListAsync();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetRoomsByCountryId")]
        public async Task<IActionResult> GetRoomsByCountryId(int countryId)
        {
            var result = await _context.Rooms.Join(
                    _context.Offices,
                    room => room.OfficeId,
                    office => office.Id,
                    (room, office) => new { Room = room, Office = office })
                .Join(
                    _context.Cities,
                    officeRoom => officeRoom.Office.CityId,
                    city => city.Id,
                    (officeRoom, city) => new { officeRoom, City = city })
                .Join(
                    _context.Countries,
                    cityOfficeRoom => cityOfficeRoom.City.CountryId,
                    country => country.Id,
                    (cityOfficeRoom, country) => new {City = cityOfficeRoom.City, Country = country }
                    )
                .Where(
                    countryCityOfficeRoom => countryCityOfficeRoom.City.CountryId == countryId)
                .ToListAsync();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("CreateRoom")]
        public async Task<IActionResult> CreateRoom(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return Created($"/GetOfficeById?id={room.Id}", room);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        [Route("UpdateRoom")]
        public async Task<IActionResult> UpdateRoom(Room roomToUpdate)
        {
            _context.Rooms.Update(roomToUpdate);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete]
        [Route("DeleteRoom{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var roomToDelete = await _context.Rooms.FindAsync(id);
            if (roomToDelete == null)
            {
                return NotFound();
            }
            _context.Rooms.Remove(roomToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}