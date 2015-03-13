using System;
using System.Collections.Generic;
using System.Net;
using xpf.Http.Original;

namespace xpf.Http
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
            this.Cookies = new HttpCookieCollection();
        }

        public HttpCookieCollection Cookies { get; set; }

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
}