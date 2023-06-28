using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace RoomsApiCrud.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string? Name { get; set; } = null;
    }
}