using System.Net;

namespace WebApiTelegramBot.Entities
{
    public class Response<TEntity>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public TEntity Data { get; set; }
        public Response(HttpStatusCode statusCode,string message,TEntity entity)
        {
           StatusCode = (int)statusCode;
           Message = message;
           Data = entity;
        }
        public Response(HttpStatusCode statusCode, string message)
        {
           StatusCode = (int)statusCode;
           Message = message;
        }
    }
}
