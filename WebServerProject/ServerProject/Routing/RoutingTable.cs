using ServerProject.Common;
using ServerProject.Http;
using ServerProject.Responses;
using HttpMethod = ServerProject.Http.HttpMethod;

namespace ServerProject.Routing
{
    public class RoutingTable : IRoutingTable
    {

        private readonly Dictionary<HttpMethod, Dictionary<string, Func<HttpRequest, HttpResponse>>> _routes;

        public RoutingTable()
        {
            this._routes = new()
            {
                [HttpMethod.Get] = new(),
                [HttpMethod.Post] = new(),
                [HttpMethod.Put] = new(),
                [HttpMethod.Delete] = new(),
            };
        }

        public IRoutingTable Map(Http.HttpMethod method, string path, HttpResponse response)
        {
            Guard.AgainstNull(response, nameof(response));

            return this.Map(method, path, request => response);
        }

        public IRoutingTable Map(Http.HttpMethod method, string path, Func<HttpRequest, HttpResponse> responseFunction)
        {
            Guard.AgainstNull(path, nameof(path));
            Guard.AgainstNull(responseFunction, nameof(responseFunction));

            this._routes[method][path.ToLower()] = responseFunction;

            return this;
        }

        public IRoutingTable MapGet(string path, HttpResponse response)
               => MapGet(path, request => response);

        public IRoutingTable MapGet(string path, Func<HttpRequest, HttpResponse> responseFunction)
                => Map(HttpMethod.Get, path, responseFunction);

        public IRoutingTable MapPost(string path, HttpResponse response)    
               => MapPost(path, request => response);

        public IRoutingTable MapPost(string path, Func<HttpRequest, HttpResponse> responseFunction)
               => Map(HttpMethod.Post, path, responseFunction);

        public HttpResponse ExecuteRequest(HttpRequest request)  // checking into the routing table do we have matching values for the HttpRequest and returning the response function 
        {
            var requestMethod = request.Method;
            var requestPath = request.Path.ToLower();

            if (!this._routes.ContainsKey(requestMethod) || !this._routes[requestMethod].ContainsKey(requestPath))
            {
                return new NotFoundResponse();
            }

            var responseFunction = this._routes[requestMethod][requestPath]; 

            return responseFunction(request);

        }
    }
}

