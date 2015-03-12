using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using xpf.Http.Original;

namespace xpf.Http
{
    public class Http
    {
        public Http()
        {
        }


        public Http(HttpMessageHandler messageHandler)
        {
            this.MessageHandler = messageHandler;
        }

        HttpMessageHandler MessageHandler { get; set; }

        public Url Url(string url)
        {
            return new Url(this.MessageHandler, url);
        }

    }

    public interface IReferenceUrl
    {
        Url Url { get; set; }
    }

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

    //public class CookieGrammar 
    //{
    //    HttpCookie CurrentCookie { get; set; }
    //    Url Url { get; set; }
    //    public CookieGrammar(Url url, string name, string value)
    //    {
    //        this.Url = url;
    //        this.CurrentCookie = new HttpCookie {Name = name, Value = value};
    //        this.Url.Model.Cookies.Add(this.CurrentCookie);
    //    }

    //    public Url And { get { return this.Url; } }

    //    public CookieGrammar ExpiryDate(DateTime expiry)
    //    {
    //        this.CurrentCookie.Expiry = expiry;
    //        return this;
    //    }
    //}

    public class RequestContentTypes : IReferenceUrl
    {
        public RequestContentTypes(Url parent, Action<IContentType> setModel)
        {
            ((IReferenceUrl)this).Url = parent;
            this.Parent = parent;
            this.SetModel = setModel;
        }

        public Url Json
        {
            get
            {
                this.SetModel(new JsonEncoder());
                return this.Parent;
            } 
        }

        public Url Xml
        {
            get
            {
                this.SetModel(new XmlEncoder());
                return this.Parent;
            }
        }
        Url Parent { get; set; }
        Action<IContentType> SetModel { get; set; }
        Url IReferenceUrl.Url { get; set; }
    }

    public class EncodingTypes : IReferenceUrl
    {
        public EncodingTypes(Url parent)
        {
            ((IReferenceUrl)this).Url = parent;
            this.Parent = parent;
        }

        
        public Url Base64
        {
            get
            {
                this.Parent.Model.Encoding = new Base64Encoder();
                return this.Parent;
            }
        }

        Url Parent { get; set; }
        Url IReferenceUrl.Url { get; set; }
    }


    public static class EncodingExtensions
    {
        public static Url Base64(this RequestContentTypes contentTypes)
        {
            var parent = ((IReferenceUrl) contentTypes).Url;
            parent.Model.Encoding = new Base64Encoder();
            return parent;
        }
    }

    public interface IContentType
    {
        string ContentType { get; }

        string Serialize<T>(T data);
        T Deserialize<T>(string data);
    }

    public interface IEncodeData
    {
        string ContentEncoding { get; }

        string Encode(string data);

        string Decode(string data);
    }

    public class JsonEncoder : IContentType
    {
        public string ContentType
        {
            get { return "application/json"; }
        }

        public string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public T Deserialize<T>(string data)
        {
            var entity = JsonConvert.DeserializeObject<T>(data);

            return entity;
        }
    }

    public class FormEncoder : IContentType
    {
        public string ContentType
        {
            get { return "application/x-www-form-urlencoded"; }
        }

        public string Serialize<T>(T data)
        {
            var formDataItems = new List<string>();
            foreach (var f in (List<HttpFormValue>)(object)data)
                formDataItems.Add(string.Format("{0}={1}", f.Key, WebUtility.UrlEncode(f.Value)));

            var formString = string.Join("&", formDataItems);

            return formString;
        }

        public T Deserialize<T>(string data)
        {
            var entity = JsonConvert.DeserializeObject<T>(data);

            return entity;
        }
    }
    public class XmlEncoder : IContentType
    {
        public string ContentType
        {
            get { return "application/xml"; }
        }

        public string Serialize<T>(T data)
        {
            string xml = "";
            using (var ms = new MemoryStream())
            {
                var xser = new XmlSerializer(typeof (T));
                xser.Serialize(ms, data);
                ms.Position = 0;
                using (var s = new StreamReader(ms))
                {
                    xml = s.ReadToEnd();
                }
            }
            return xml;
        }

        public T Deserialize<T>(string data)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                XmlSerializer xser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                var entity = xser.Deserialize(ms);
                return (T)entity;
            }
        }
    }

    public class Base64Encoder : IEncodeData
    {
        public string ContentType
        {
            get { return "text/html"; }
        }

        public string ContentEncoding
        {
            get { throw new NotImplementedException(); }
        }

        public string Encode(string data)
        {
            var bytesToEncode = Encoding.Unicode.GetBytes(data as string);
            return Convert.ToBase64String(bytesToEncode);
        }

        public string Decode(string data)
        {
            var decodedBytes = Convert.FromBase64String(data);
            return Encoding.Unicode.GetString(decodedBytes, 0, decodedBytes.Length);
        }
    }

    public class StringEncoder : IContentType
    {
        public string ContentType
        {
            get { return "text/html"; }
        }

        public string Serialize<T>(T data)
        {
            return data as string;
        }

        public T Deserialize<T>(string data)
        {
            return (T)(data as object);
        }
    }
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
            this.Cookies = new List<HttpCookie>();
        }

        public List<HttpCookie> Cookies { get; set; }

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

        /// <summary>
        /// Provided a regular expression will evaluate it agains the raw result
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Scrape Scrape(string expression)
        {
            if (this.StatusCode == HttpStatusCode.OK)
            {
                return new Scrape(expression, this.RawContent);
            }

            throw new ArgumentException("Scrape is only supported with a StatusCode of OK (200)");
        }
    }

    public class Scrape
    {
        string Expression { get; set; }
        string Content { get; set; }

        ExpressionGroupCollection Results { get; set; }

        public Scrape(string expression, string content)
        {
            this.Expression = expression;
            this.Content = content;
            this.Results = this.ScrapExpression(expression);
        }

        public Scrape And(string expression)
        {
            this.Expression = expression;
            foreach (var g in this.ScrapExpression(expression))
                this.Results.Add(g);

            return this;
        }

        public ExpressionGroupCollection Result()
        {
            return this.Results;
        }

        /// <summary>
        /// Provided a regular expression will evaluate it agains the raw result
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ExpressionGroupCollection ScrapExpression(string expression)
        {
            // https://msdn.microsoft.com/en-us/library/system.text.regularexpressions.regex.groupnamefromnumber(v=vs.110).aspx
            // Use above sampel to rewrite this taking advantage of the groups names. This needs its own method
            var foundValues = new ExpressionGroupCollection();

            Regex regex = new Regex(expression);
            List<string> groupNames = new List<string>();
            int index = 1;
            bool nameNotFound = false;
            // Get group names. 
            do
            {
                string name = regex.GroupNameFromNumber(index);
                if (!String.IsNullOrEmpty(name))
                {
                    index++;
                    groupNames.Add(name);
                }
                else
                {
                    nameNotFound = true;
                }
            } while (!nameNotFound);


            var match = regex.Matches(this.Content);
            foreach (string group in groupNames)
            {
                var expressionGroup = new ExpressionGroup();
                // Incase a subsequent expression uses the same group name use the previous collection
                if (this.Results != null && this.Results.Contains(group))
                    expressionGroup = this.Results[group];

                expressionGroup.Name = group;
                if (match.Count > 0)
                    for (int i = 0; i < match.Count; i++)
                    {
                        var values = match[i].Groups[group];
                        for (int c = 0; c < values.Captures.Count; c++)
                        {
                            expressionGroup.Values.Add(values.Captures[c].Value);
                        }
                    }
                foundValues.Add(expressionGroup);
            }

            return foundValues;

            //if (matches.Count == 2)
            //{
            //    classification = new WebSiteClassification
            //    {
            //        SecurityRisk = matches[0].Groups[1].Captures[0].Value,
            //        Category = matches[1].Groups[1].Captures[0].Value
            //    };
            //}
        }

    }

}

