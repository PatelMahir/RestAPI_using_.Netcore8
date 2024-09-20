using System.Net;
namespace RestAPI_using_.Netcore8.Models.DTOs
{
    public class ResponseApi
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result {  get; set; }
        public ResponseApi()
        {
            ErrorMessages=new List<string>();
        }
    }
}