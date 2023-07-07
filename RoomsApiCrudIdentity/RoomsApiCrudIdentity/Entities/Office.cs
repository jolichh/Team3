using Microsoft.AspNetCore.Mvc;

namespace RoomsApiCrudIdentity.Entities
{
    public class Office
    {
        public int Id { get; set; } = -1;
        public string? Name { get; set; } = null;
        public int CityId { get; set; } = -1;
    }
}