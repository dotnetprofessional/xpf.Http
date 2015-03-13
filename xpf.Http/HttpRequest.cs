using System.Collections.Generic;
using System.Net.Http;
using xpf.Http.Original;

namespace xpf.Http
{
    public class HttpRequest
    {
        public HttpRequest()
        {
            this.Headers = new List<HttpHeader>();
            this.Cookies = new List<HttpCookie>();
            this.FormValues = new List<HttpFormValue>();
            this.Headers.Add(new HttpHeader {Key = "Accept", Value = new List<string> {"text/html"}});
            //this.Headers.Add(new HttpHeader {Key = "Accept-Encoding", Value = new List<string> {"gzip, deflate"}});
            this.AllowAutoRedirect = true;

            this.RequestContentType = new StringEncoder();
            this.ResponseContentType= new StringEncoder();

            this.KnownContentTypes = new List<IContentType>();
            // Register the known content types for auto-evaluation. 
            this.KnownContentTypes.Add(new StringEncoder());
            this.KnownContentTypes.Add(new JsonEncoder());
            this.KnownContentTypes.Add(new XmlEncoder());
            this.KnownContentTypes.Add(new FormEncoder());
        }

        public List<IContentType> KnownContentTypes { get; set; }

        public string Url { get; set; }

        public List<HttpHeader> Headers { get; private set; }

        public List<HttpCookie> Cookies { get; private set; }

        public bool AllowAutoRedirect { get; set; }
        public IContentType RequestContentType { get; set; }
        public IContentType ResponseContentType { get; set; }

        public bool EnableExpectContinue { get; set; }
        public IEncodeData Encoding { get; set; }
        public List<HttpFormValue> FormValues { get; set; }
        public object Data { get; set; }
        public StringContent Content { get; set; }
    }
}