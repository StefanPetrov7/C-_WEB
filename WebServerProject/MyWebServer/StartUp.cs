using MyWebServerApp.Controllers;
using ServerProject;
using ServerProject.Controllers;

namespace MyWebServerApp
{
    public class StartUp
    {
        public static async Task Main()
            => await new HttpServer(routes => routes
            .MapGet<HomeController>("/", controller => controller.Index())
            .MapGet<HomeController>("/softuni", controller => controller.ToSoftUni())
            .MapGet<AnimalsController>("/Cats", controller => controller.Cats())
            .MapGet<AnimalsController>("/Dogs", controller => controller.Dogs()))
            .Start();
    }
}
