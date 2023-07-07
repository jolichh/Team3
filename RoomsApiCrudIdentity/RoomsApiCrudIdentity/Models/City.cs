using Microsoft.AspNetCore.Mvc;

namespace RoomsApiCrudIdentity.Models
{
    public class City : IModel
    {
        public int Id { get; set; } = -1;
        public string? Name { get; set; } = null;
        public int CountryId { get; set; } = -1;
    }
}