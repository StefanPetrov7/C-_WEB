
using ServerProject.Http;

namespace ServerProject.Responses
{
    public class NotFoundResponse : HttpResponse
    {

        public NotFoundResponse() : base(HttpStatusCode.NotFound)
        {
            
        }




    }
}
