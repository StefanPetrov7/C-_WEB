
using ServerProject.Http;

namespace ServerProject.Responses
{
    public class BadRequestResponse : HttpResponse
    {
        public BadRequestResponse() : base(HttpStatusCode.BadRequest) // giving bad request status code to the parent class HttpResponse
        {
            
        }
    }
}
