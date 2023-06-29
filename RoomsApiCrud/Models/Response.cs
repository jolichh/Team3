namespace RoomsApiCrud.Models
{
    public class Response
    {
        public int StatusCode {get; set;} = -1;
        public string? StatusMessage {get; set;}
        public IModel? IndividualResult {get; set;} = null;
        public List<IModel>? ResultList {get; set;} = null;
    }
}