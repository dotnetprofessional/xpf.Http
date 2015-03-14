using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using xpf.Http.Original;

namespace xpf.Http
{
    public class FormContent : IContentType
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
}