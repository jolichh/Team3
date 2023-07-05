using Microsoft.AspNetCore.Mvc;

namespace RoomsApiCrud.Models
{
    public class Country : IModel
    {
        public int Id { get; set; } = -1;
        public string? Name { get; set; } = null;
    }
}