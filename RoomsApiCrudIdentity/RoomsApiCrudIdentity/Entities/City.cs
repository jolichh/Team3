using Microsoft.AspNetCore.Mvc;

namespace RoomsApiCrudIdentity.Entities
{
    public class City
    {
        public int Id { get; set; } = -1;
        public string? Name { get; set; } = null;
        public int CountryId { get; set; } = -1;
    }
}