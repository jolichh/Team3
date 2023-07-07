using Microsoft.AspNetCore.Mvc;

namespace RoomsApiCrudIdentity.Models
{
    public class Room : IModel
    {
        public int Id { get; set; } = -1;
        public string? Name { get; set; } = null;
        public int OfficeId { get; set; } = -1;
    }
}