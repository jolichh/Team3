namespace RoomsApiCrudIdentity.Models
{
    public class Response<T>
    {
        public int StatusCode {get; set;} = -1;
        public string? StatusMessage {get; set;}
        public T? Result {get; set;}
    }
}