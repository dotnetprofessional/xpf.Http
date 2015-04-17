using System.Collections.Generic;
using System.Net.Http;
using xpf.Http.Original;

namespace xpf.Http
{
    public class HttpRequest
    {
        public HttpRequest()
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            this.FormValues = new List<HttpFormValue>();
            this.AllowAutoRedirect = true;
            this.EnableExpectContinue = true;

            this.RequestContentType = new StringContent();
            this.ResponseContentType= new StringContent();
            this.Encoding = new NullEncoder();

            // Register the known content types for auto-evaluation. 
            this.KnownContentTypes = new KnownContentTypesCollection
            {
                new StringContent(),
                new JsonContent(),
                new XmlContent(),
                new FormContent()
            };
        }

        public KnownContentTypesCollection KnownContentTypes { get; set; }

        public string Url { get; set; }

        public HttpHeaderCollection Headers { get; private set; }

        public HttpCookieCollection Cookies { get; private set; }

        public bool AllowAutoRedirect { get; set; }
        public IContentType RequestContentType { get; set; }
        public IContentType ResponseContentType { get; set; }

        public bool EnableExpectContinue { get; set; }
        public IEncodeData Encoding { get; set; }
        public List<HttpFormValue> FormValues { get; set; }
        public object Data { get; set; }
        public System.Net.Http.StringContent Content { get; set; }
    }
}