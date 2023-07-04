namespace RoomsApiCrud.Models
{
    public class ResponseFactory
    {
        public static Response<IModel?> Create500()
        {
            return new Response<IModel?>()
            {
                StatusCode = 500,
                StatusMessage = "Operation failed.",
                Result = null
            };
        }

        public static Response<IModel> CreateSingleResultSuccess(IModel? result, int code)
        {
            return new Response<IModel>()
            {
                StatusCode = code,
                StatusMessage = "Success",
                Result = result!
            };
        }

        public static Response<List<IModel>> CreateListResultSuccess(List<IModel>? result, int code)
        {
            return new Response<List<IModel>>()
            {
                StatusCode = code,
                StatusMessage = "Success",
                Result = result!
            };
        }
    }
}