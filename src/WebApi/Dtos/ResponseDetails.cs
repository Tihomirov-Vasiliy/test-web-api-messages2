using System.Text.Json;

namespace WebApi.Dtos
{
    public class ResponseDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ResponseDetails(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
