using System;
using System.Collections.Generic;
using System.Text;
using Machine.Specifications.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy;
using Nancy.Cookies;
using Nancy.ModelBinding;
using Nancy.Responses;
using Newtonsoft.Json;
using xpf.Http.Spec.TestModels;

namespace xpf.Http.Spec
{
    public class SpecHttpModule : NancyModule
    {
        public SpecHttpModule()
        {
            #region Get Methods

            Get["/When_requesting_a_valid_url_that_is_not_a_html_page_as_text"] = _ => "this is simple text";

            Get["/basic-url-that-returns-a-simple-html-body"] = _ => "<html><meta name=\"description\" content=\"test description\" <title>test title</title> /></html>";

            Get["/basic-url-that-returns-a-simple-json-document"] = delegate
            {
                var c = new Customer { Name = "Test", Phone = "12345678" };
                return JsonConvert.SerializeObject(c);
            };

            Get["/basic-url-that-returns-a-simple-base64-document"] = delegate
            {
                var encoder = new Base64Encoder();
                return encoder.Encode("base64 Text").Result;
            };

            Get["/basic-url-that-returns-a-simple-xml-document"] = p =>
            {
                var c = new Customer { Name = "Test", Phone = "12345678" };
                var encoder = new XmlContent();

                var r = new Response
                {
                    StatusCode = HttpStatusCode.OK,
                    Contents = (s) =>
                    {
                        var entity = encoder.Serialize(c);
                        var data = Encoding.UTF8.GetBytes(entity);
                        s.Write(data, 0, data.Length);
                    }
                };

                r.Headers.Add("Content-Type", "application/xml");

                return r;
            };

            Get["/basic-url-that-returns-headers-as-keyvalue-pairs"] = delegate
            {
                // return the headers as a keyvalue pair
                var headers = new List<string>();
                foreach (var h in this.Request.Headers)
                    headers.Add(string.Format("{0}:{1}", h.Key, h.Value));

                return JsonConvert.SerializeObject(headers);
            };

            Get["/basic-url-that-returns-a-header"] = delegate
            {
                // return the headers as a keyvalue pair
                var headers = new List<string>();
                foreach (var h in this.Request.Headers)
                    headers.Add(string.Format("{0}:{1}", h.Key, h.Value));

                return JsonConvert.SerializeObject(headers);
            };

            Get["/basic-url-recieves-and-sets-cookies"] = delegate
            {
                // return the same headers that were given. The TestServer doesn't allow setting
                // cookies
                var cookies = new List<string>();
                foreach (var h in this.Request.Cookies)
                    this.Context.Response.Cookies.Add(new NancyCookie(h.Key, h.Value));

                // set a cookie too
                return "set cookies";
            }; 

            #endregion

            #region POST Methods

            Post["/basic-url-that-accepts-json-document"] = p =>
            {
                var c = this.Bind<Customer>();
                if (c != null && !string.IsNullOrEmpty(c.Name))
                    return HttpStatusCode.OK;
                else
                    return HttpStatusCode.Forbidden;
            };
            #endregion
        }
    }
}
