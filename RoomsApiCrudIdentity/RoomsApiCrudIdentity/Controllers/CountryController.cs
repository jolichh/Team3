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
    public class CountryController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        private readonly RoomsApiCrudDbContext _context;

        public CountryController(IConfiguration configuration, RoomsApiCrudDbContext context)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("RoomsApiCrudConn")!;
            _context = context;
        }

        [HttpGet]
        [Route("GetAllCountries")]
        public async Task<IActionResult> GetAllCountries()
        {
            return Ok(await _context.Countries.ToListAsync());
        }

        [HttpGet]
        [Route("GetCountryById")]
        public async Task<IActionResult> GetCountryById(int id)
        {
            return Ok(await _context.Countries.FindAsync(id));
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("CreateCountry")]
        public async Task<IActionResult> CreateCountry(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return Created($"/GetCountryById?id={country.Id}", country);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        [Route("UpdateCountry")]
        public async Task<IActionResult> UpdateCountry(Country countryToUpdate)
        {
            _context.Countries.Update(countryToUpdate);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete]
        [Route("DeleteCountry")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var countryToDelete = await _context.Countries.FindAsync(id);
            if (countryToDelete == null)
            {
                return NotFound();
            }
            _context.Countries.Remove(countryToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}