using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using xpf.Http.Original;

namespace xpf.Http
{
    public class Url
    {
        public Url(HttpMessageHandler messageHandler, string url)
        {
            this.MessageHandler = messageHandler;
            this.Model = new HttpRequest {Url = url};
            this.RequestContentType = new RequestContentTypes(this, e => this.Model.RequestContentType = e);
            this.ResponseContentType = new RequestContentTypes(this, e => this.Model.ResponseContentType= e);
            this.Encoding = new EncodingTypes(this);

        }

        public HttpRequest Model { get; private set; }

        HttpClient initializeClientHttpHandler()
        {
            HttpClient client;
            if (this.MessageHandler == null)
            {
                this.MessageHandler = new HttpClientHandler();
                client = new HttpClient(this.MessageHandler);
            }
            else
            {
                client = new HttpClient(this.MessageHandler)
                {
                    BaseAddress = new Uri("http://www.dummysite.com")
                };
            }

            return client;
        }

        HttpMessageHandler MessageHandler { get; set; }
        public RequestContentTypes RequestContentType { get; private set; }

        public RequestContentTypes ResponseContentType { get; private set; }

        public EncodingTypes Encoding{ get; private set; }

        public Url WithoutRedirect
        {
            get
            {
                this.Model.AllowAutoRedirect = false;
                return this;
            }
        }

        public async Task<HttpResponse<TR>> PostAsync<TR>()
        {
            return await this.PostAsync<TR>(null);
        }

        public async Task<HttpResponse<TR>> PostAsync<TR>(object data)
        {
            HttpResponseMessage response;
            HttpClient client = this.InitializeClientRequest();
            this.Model.Data = data;

            response = await client.PostAsync(this.Model.Url, this.Model.Content);

            return await this.ProcessResponse<TR>(response);
        }

        public async Task<HttpResponse<TR>> GetAsync<TR>()
        {
            HttpResponseMessage response;
            HttpClient client = this.InitializeClientRequest();

            response = await client.GetAsync(this.Model.Url);

            return await this.ProcessResponse<TR>(response);
        }

        async Task<HttpResponse<TR>> ProcessResponse<TR>(HttpResponseMessage response)
        {
            string error = "";
            var result = default(TR);
            var rawContent = await new StreamReader(await response.Content.ReadAsStreamAsync()).ReadToEndAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // If an encoding has been specified make use of that to first decode the data
                // before attempting to convert the type
                // TOOD: Should probably look at the response headers content-encoding/content-type to determine how to work with the response
                var decoded = rawContent;
                foreach(var h in response.Headers)
                    if (h.Key == "Content-Type")
                    {
                        var value = new List<string>(h.Value)[0];

                        // Find a matching content type decoder
                        foreach (var c in this.Model.KnownContentTypes)
                            if (c.ContentType == value)
                                this.Model.ResponseContentType = c;
                        break;
                    }
                if (this.Model.Encoding != null)
                    decoded = this.Model.Encoding.Decode(decoded);

                result = this.Model.ResponseContentType.Deserialize<TR>(decoded);
            }
            else
                error = rawContent;

            // Depending on the format c
            var requestResponse = new HttpResponse<TR>(this.Model.Url, response.StatusCode, result, error, rawContent);
            foreach (var header in response.Headers)
            {
                if (header.Key == "Set-Cookie")
                {
                    var cookies = this.ExtractCookies(header.Value);
                    requestResponse.Cookies.AddRange(cookies);
                }
                else
                    requestResponse.Headers.Add(header.Key, new HttpHeader {Key = header.Key, Value = new List<string>(header.Value)});
            }

            return requestResponse;
        }

        HttpClient InitializeClientRequest()
        {
            var client = this.initializeClientHttpHandler();

            client.DefaultRequestHeaders.ExpectContinue = this.Model.EnableExpectContinue;

            StringContent content = null;
            // Check that Forms data and raw data have not both been set. As you can't have both
            if(this.Model.FormValues.Count != 0 && this.Model.Data != null)
                throw new ArgumentException("Setting both FormValues and Data is not supported. Only one can be set at a time.");

            if (this.Model.FormValues.Count > 0)
            {
                this.Model.RequestContentType = new FormEncoder();
                this.Model.Data = this.Model.FormValues;
            }
            content = new StringContent(this.Model.RequestContentType.Serialize(this.Model.Data ?? ""));

            foreach (HttpHeader h in this.Model.Headers)
            {
                client.DefaultRequestHeaders.Add(h.Key, h.Value);
            }
            content.Headers.ContentType = new MediaTypeHeaderValue(this.Model.RequestContentType.ContentType);
            // Can't work out how to get the TestServer to support cookies
            
            if (this.MessageHandler is HttpClientHandler)
            {
                var clientHandler = (HttpClientHandler) this.MessageHandler;
                if (this.Model.Cookies.Count > 0)
                {
                    var cookieContainer = new CookieContainer();
                    foreach (HttpCookie c in this.Model.Cookies)
                        cookieContainer.Add(new Uri(this.Model.Url), new Cookie(c.Name, c.Value));
                    clientHandler.CookieContainer = cookieContainer;
                }

                clientHandler.AllowAutoRedirect = this.Model.AllowAutoRedirect;
            }

            // Set the value of content to the model 
            this.Model.Content = content;
            return client;
        }

        private List<HttpCookie> ExtractCookies(IEnumerable<string> header)
        {
            var httpCookies = new List<HttpCookie>();
            foreach (string cookie in header)
            {
                httpCookies.Add(new HttpCookie(cookie));
            }
            return httpCookies;
        }
        public Url WithHeader(string name, IEnumerable<string> value)
        {
            this.Model.Headers.Add(new HttpHeader{Key = name, Value = value});
            return this;
        }

        public Url WithCookie(string name, string value, DateTime expiry = default(DateTime), string domain = null, string path = null)
        {
            return this.WithCookie(new HttpCookie { Domain = domain, Name = name, Value = value, Expiry = expiry, Path = path });
        }

        public Url WithCookie(HttpCookie cookie)
        {
            this.Model.Cookies.Add(cookie);
            return this;
        }

        public Url WithFormValue(string name, string value)
        {
            this.Model.FormValues.Add(new HttpFormValue {Key = name, Value = value});
            return this;
        }

    }
}