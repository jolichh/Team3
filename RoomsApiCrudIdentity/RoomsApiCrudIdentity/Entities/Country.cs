using Microsoft.AspNetCore.Mvc;

namespace RoomsApiCrudIdentity.Entities
{
    public class Country
    {
        public int Id { get; set; } = -1;
        public string? Name { get; set; } = null;
    }
}