using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Data;

using RoomsApiCrudIdentity.Data;
using RoomsApiCrudIdentity.Entities;

namespace RoomsApiCrudIdentity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        private readonly RoomsApiCrudDbContext _context;
        
        public CityController(IConfiguration configuration, RoomsApiCrudDbContext context)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn")!;
            _context = context;
        }

        [HttpGet]
        [Route("GetAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            return Ok(await _context.Cities.ToListAsync());
        }

        [HttpGet]
        [Route("GetCityById")]
        public async Task<IActionResult> GetCityById(int id)
        {
            return Ok(await _context.Cities.FindAsync(id));
        }

        [HttpGet]
        [Route("GetCitiesByCountryId")]
        public async Task<IActionResult> GetCitiesByCountryId(int countryId)
        {
            var result = await _context.Cities.Where(x => x.CountryId == countryId).ToListAsync();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(await _context.Cities.Where(x => x.CountryId == countryId).ToListAsync());
        }

        [HttpPost]
        [Route("CreateCity")]
        [HttpPost]
        public async Task<IActionResult> CreateCity(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
            return Created($"/GetCityById?id={city.Id}", city);
        }

        [HttpPut]
        [Route("UpdateCity")]
        public async Task<IActionResult> UpdateCity(City cityToUpdate)
        {
            _context.Cities.Update(cityToUpdate);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteCity{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var cityToDelete = await _context.Cities.FindAsync(id);
            if (cityToDelete == null)
            {
                return NotFound();
            }
            _context.Cities.Remove(cityToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}