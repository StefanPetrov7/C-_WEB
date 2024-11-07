using ServerProject.Http;
using ServerProject.Routing;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerProject
{
    public class HttpServer
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly TcpListener listener;

        private readonly RoutingTable _routingTable;
        public HttpServer(string ipAddress, int port, Action<IRoutingTable> routingTableConfiguration)  // Http class which allowing us to start server. Also taking Action for parameter to allows us to chain Methods
        {
            this._ipAddress = IPAddress.Parse(ipAddress);
            this._port = port;

            this.listener = new TcpListener(this._ipAddress, this._port);

            routingTableConfiguration(this._routingTable = new RoutingTable());
        }

        public HttpServer(int port, Action<IRoutingTable> routingTableConfiguration) : this("127.0.0.1", port, routingTableConfiguration)  // Second constructor with default ipAddress
        {

        }

        public HttpServer(Action<IRoutingTable> routingTableConfiguration) : this(5000, routingTableConfiguration)  // Third constructor with default port
        {

        }

        public async Task Start()
        {
            this.listener.Start();

            Console.WriteLine($"Server started on port {this._port}");
            Console.WriteLine("Listening for requests...");

            while (true)  
            {

                var connection = await this.listener.AcceptTcpClientAsync();  // connecting to the browser

                var networkStream = connection.GetStream();  // two way stream between the browser and the server (data is in bytes)

                var requestText = await ReadRequest(networkStream);  // Reading the request

                //  Console.WriteLine(requestText);

                var request = HttpRequest.Parse(requestText);

                var response = this._routingTable.ExecuteRequest(request);

                await WriteResponse(networkStream, response);  // Writing response to the browser 

                connection.Close();
            }
        }

        private async Task<string> ReadRequest(NetworkStream networkStream)  // This method will read the browser request to the server
        {

            var bufferLength = 1024;  // reading data on pieces of 1MB 

            var buffer = new byte[bufferLength];

            var totalBytesRead = 0;

            var requestBuilder = new StringBuilder();

            do
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);  // writing the data into the 1MB buffer before appending it to the string builder

                totalBytesRead += bytesRead;

                if (totalBytesRead > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is to large");
                }

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));  // writing all the data into a string builder encoded from bytes to string 

            }
            while (networkStream.DataAvailable);

            return requestBuilder.ToString();
        }

        private async Task WriteResponse(NetworkStream networkStream ,HttpResponse response)
        {

            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());  // to send data they need to be parsed to byte[]

            await networkStream.WriteAsync(responseBytes);  // send response 

        }
    }
}
