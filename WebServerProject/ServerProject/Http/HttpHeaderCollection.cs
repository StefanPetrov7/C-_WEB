using System.Collections;

namespace ServerProject.Http
{
    public class HttpHeaderCollection : IEnumerable<HttpHeader>
    {
        private readonly Dictionary<string, HttpHeader> _headers;

        public HttpHeaderCollection()
            => this._headers = new Dictionary<string, HttpHeader>();

        public int Count => this._headers.Count;

        public void Add(string name, string value)
        {
            var header = new HttpHeader(name, value);
            this._headers.Add(name, header);
        }

        public IEnumerator<HttpHeader> GetEnumerator()
            => this._headers.Values.GetEnumerator();    
      

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

    }
}
