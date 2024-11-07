
namespace ServerProject.Http
{
    public class HttpRequest
    {
        private const string NewLine = "\r\n";

        public HttpMethod Method { get; private set; }

        public string Path { get; private set; }

        public Dictionary<string, string> Query { get; private set; }

        public HttpHeaderCollection Headers { get; private set; }

        public string Body { get; private set; }

        public static HttpRequest Parse(string request)   // Parsing the HTTP request 
        {
            var lines = request.Split(NewLine);        // Taking all lines from the request 

            var startLine = lines.First().Split(" ");  // Taking only the first line with => with Method / URL / HTTP version 

            var method = ParseHttpMethod(startLine[0]);  // Parsing the HTTP method

            var url = startLine[1];

            var (path, query) = ParseUrl(url);  // using returning Tuple method to get two variable

            var headerCollection = ParseHttpHeaders(lines.Skip(1));  // Parsing the headers 

            var bodyLines = lines.Skip(headerCollection.Count + 2).ToArray();

            var body = string.Join(NewLine, bodyLines);

            return new HttpRequest
            {
                Method = method,
                Path = path,
                Query = query,
                Headers = headerCollection,
                Body = body
            };
        }

        private static HttpMethod ParseHttpMethod(string method)
        {
            return method.ToUpper() switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                _ => throw new InvalidOperationException($"Method '{method.GetType().Name}' in not valid")
            };
        }

        private static (string, Dictionary<string, string>) ParseUrl(string url)  // Parsing the URL => returning Tuple from the query string, and query if exists 
        {
            var urlParts = url.Split('?', 2);

            var path = urlParts[0];
            var query = urlParts.Length > 1 ? ParseQuery(urlParts[1]) : new Dictionary<string, string>();

            return (path, query);
        }

        private static Dictionary<string, string> ParseQuery(string queryString)  // splitting the query string
        {
            return queryString
                   .Split('&')
                   .Select(part => part.Split('='))
                   .Where(part => part.Length == 2)
                   .ToDictionary(part => part[0], part => part[1]);
        }

        private static HttpHeaderCollection ParseHttpHeaders(IEnumerable<string> headerLines)
        {
            var headerCollection = new HttpHeaderCollection();

            foreach (var headerLine in headerLines)
            {
                if (headerLine == string.Empty)
                {
                    break;
                }

                var headerParts = headerLine.Split(":", 2);

                if (headerParts.Length != 2)
                {
                    throw new InvalidOperationException("Request is not valid");
                }

                var headerName = headerParts[0];
                var headerValue = headerParts[1].Trim();

                var header = new HttpHeader(headerName, headerValue);


                headerCollection.Add(headerName, headerValue);
            }

            return headerCollection;
        }
    }
}
