using System.Collections.Generic;
using System.Net;

namespace xpf.Http.Original
{
    public class HttpResponse<T>
    {
        UriDetail _detail;

        public HttpResponse(string url, HttpStatusCode statusCode, T content, string error, string rawContent)
        {
            this.Headers = new Dictionary<string, HttpHeader>();
            this.StatusCode = statusCode;
            this.Content = content;
            this.Error = error;
            this.Url = url;
            this.RawContent = rawContent;
        }

        public string Url { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public T Content { get; private set; }

        public string Error { get; set; }

        public Dictionary<string, HttpHeader> Headers { get; set; }

        public string RawContent { get; private set; }

        public UriDetail Detail
        {
            get
            {
                if (this._detail == null)
                    this._detail = new UriDetail(this.Url, this.RawContent);

                return this._detail;
            }
            private set { this._detail = value; }
        }
    }
}