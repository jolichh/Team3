namespace RoomsApiCrud.Models
{
    public class CountryResponse
    {
        public int StatusCode {get; set;}
        public string StatusMessage {get; set;}
        public Country country {get; set;}
        public List<Country> countryList {get; set;}
    }
}