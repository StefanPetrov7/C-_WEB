using ServerProject.Http;
using ServerProject.Routing;

namespace ServerProject.Controllers
{
    public static class RoutingExtensions
    {
        public static IRoutingTable MapGet<TController>(this IRoutingTable routingTable, string path, Func<TController, HttpResponse> controllerFunction)
            where TController : Controller
        {
            return routingTable.MapGet(path, request =>
            {
                var controller = CreateController<TController>(request);

                return controllerFunction(controller);
            });
        }

        public static IRoutingTable MapPost<TController>(this IRoutingTable routingTable, string path, Func<TController, HttpResponse> controllerFunction)
            where TController : Controller
        {
            return routingTable.MapPost(path, request =>
            {
                var controller = CreateController<TController>(request);

                return controllerFunction(controller);
            });
        }

        private static TController CreateController<TController>(HttpRequest request) 
        {
            return (TController)Activator.CreateInstance(typeof(TController), new[] { request });
        }
    }
}
