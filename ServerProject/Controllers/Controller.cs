using ServerProject.Http;
using ServerProject.Responses;

namespace ServerProject.Controllers
{
    public abstract class Controller
    {
        protected Controller(HttpRequest request)
        {
            this.Request = request;
        }

        protected HttpRequest Request { get; private init; }

        protected HttpResponse Text(string text) 
        {
            return new TextResponse(text);
        }

        protected HttpResponse Html(string html)
        {
            return new HtmlResponse(html);
        }

        protected HttpResponse Redirect(string location) 
        {
           return new RedirectResponse(location);   
        }
    }
}
