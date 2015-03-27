using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using xpf.Http.Original;

namespace xpf.Http
{
    public class HttpResponse<T>
    {
        PageDetail<T> _detail;
        NavigationContext Parent { get; set; }

        public HttpResponse()
        {
        }

        public HttpResponse(NavigationContext parent, HttpStatusCode statusCode, T content, string error, string rawContent)
        {
            this.Headers = new HttpHeaderCollection();
            this.Parent = parent;
            this.StatusCode = statusCode;
            this.Content = content;
            this.Error = error;
            this.Url = parent.Model.Url;
            this.RawContent = rawContent;
            this.Cookies = new HttpCookieCollection();
        }

        public HttpCookieCollection Cookies { get; set; }

        public string Url { get; private set; }

        public HttpStatusCode StatusCode { get; set; }

        public T Content { get; private set; }

        public string Error { get; set; }

        public HttpHeaderCollection Headers { get; set; }

        public string RawContent { get; set; }

        public PageDetail<T> Detail
        {
            get
            {
                if (this._detail == null)
                    this._detail = new PageDetail<T>(this);

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

        public NavigationContext Navigate()
        {
            return this.Navigate(this.Parent.Model.Url);
        }

        public NavigationContext Navigate(string url)
        {
            var currentModel = this.Parent.Model;
            // Clear out the existing model ready for the new request
            this.Parent.Model = new HttpRequest();

            var model = this.Parent.Model;
            model.Url = url;
            this.Parent.MessageHandler = null;

            // Copy all of the relevant details from the previous response to the model
            if (currentModel.Headers.Contains("User-Agent")) model.Headers.Add(currentModel.Headers["User-Agent"]);
            if (this.Headers.Contains("Accept")) model.Headers.Add(this.Headers["Accept"]);
            if (this.Headers.Contains("Content-Type")) model.Headers.Add(this.Headers["Content-Type"]);
            var clientIpHeaders = currentModel.Headers.Where(h => h.Key.Contains("Forward") || h.Key.Contains("Client")).ToList();
            if(clientIpHeaders != null && clientIpHeaders.Count > 0)
                model.Headers.AddRange(clientIpHeaders);
            
            // Set the referrer
            this.Parent.WithReferrer(currentModel.Url);

            foreach (var c in this.Cookies)
                model.Cookies.Add(c);

            return this.Parent;
        }
    }
}