using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Data;

using RoomsApiCrudIdentity.Data;
using RoomsApiCrudIdentity.Entities;

namespace RoomsApiCrudIdentity.Controllers
{
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
            var result = await _context.Rooms.Where(x => x.OfficeId == officeId).ToListAsync();
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
                    (officeRoom, city) => new { CityId = officeRoom.Office.CityId, Id = city.Id })
                .Where(
                    cityOfficeRoom => cityOfficeRoom.Id == cityId)
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
                    (cityOfficeRoom, country) => new {CountryId = cityOfficeRoom.City.CountryId, Id = country.Id }
                    )
                .Where(
                    countryCityOfficeRoom => countryCityOfficeRoom.Id == countryId)
                .ToListAsync();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateOffice")]
        [HttpPost]
        public async Task<IActionResult> CreateOffice(Office office)
        {
            _context.Offices.Add(office);
            await _context.SaveChangesAsync();
            return Created($"/GetOfficeById?id={office.Id}", office);
        }

        [HttpPut]
        [Route("UpdateOffice")]
        public async Task<IActionResult> UpdateOffice(Office officeToUpdate)
        {
            _context.Offices.Update(officeToUpdate);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteOffice{id}")]
        public async Task<IActionResult> DeleteOffice(int id)
        {
            var officeToDelete = await _context.Offices.FindAsync(id);
            if (officeToDelete == null)
            {
                return NotFound();
            }
            _context.Offices.Remove(officeToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}