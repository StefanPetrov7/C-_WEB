using ServerProject.Http;

namespace ServerProject.Responses
{
    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string location) : base(HttpStatusCode.Found)
        {
            this.Headers.Add("Location", location);
        }
    }
}
