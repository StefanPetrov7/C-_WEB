
using ServerProject.Controllers;
using ServerProject.Http;
using ServerProject.Responses;

namespace MyWebServerApp.Controllers
{
    public class HomeController : Controller
    {

        public HomeController(HttpRequest request) : base(request)
        {
        }
             
        public HttpResponse Index() => Text("Hello");

        public HttpResponse ToSoftUni() => Redirect("https://softuni.bg");

    }
}
