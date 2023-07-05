using Microsoft.AspNetCore.Mvc;

namespace RoomsApiCrud.Models
{
    public class Office : IModel
    {
        public int Id { get; set; } = -1;
        public string? Name { get; set; } = null;
        public int CityId { get; set; } = -1;
    }
}