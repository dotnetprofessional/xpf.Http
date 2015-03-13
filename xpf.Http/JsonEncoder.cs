using Newtonsoft.Json;

namespace xpf.Http
{
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
}