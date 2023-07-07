using Microsoft.AspNetCore.Mvc;

namespace RoomsApiCrudIdentity.Entities
{
    public class Room
    {
        public int Id { get; set; } = -1;
        public string? Name { get; set; } = null;
        public int OfficeId { get; set; } = -1;
    }
}