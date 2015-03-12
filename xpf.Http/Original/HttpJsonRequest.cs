using System.Collections.Generic;
using Newtonsoft.Json;

namespace xpf.Http.Original
{
    public class HttpJsonRequest : HttpRequest
    {
        public HttpJsonRequest()
        {
            this.RequestFormat = HttpFormat.JSON;
            this.Headers.Add(new HttpHeader { Key = "Accept", Value = new List<string> { "application/json" } });
            this.Headers.Add(new HttpHeader { Key = "Content-Type", Value = new List<string> { "application/json" } });
        }

        public override string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public override T Deserialize<T>(string data)
        {
            var entity = JsonConvert.DeserializeObject<T>(data);

            return entity;
        }
    }
}