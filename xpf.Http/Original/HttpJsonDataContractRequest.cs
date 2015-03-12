﻿using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace xpf.Http.Original
{
    public class HttpJsonDataContractRequest : HttpJsonRequest
    {
        public override string Serialize<T>(T data)
        {
            var ser = new DataContractJsonSerializer(data.GetType());
            string json = "";
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, data);
                var bytes = ms.ToArray();
                json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
            return json;
        }

        public override T Deserialize<T>(string data)
        {
            var ser = new DataContractJsonSerializer(data.GetType());
            using (var ms = new MemoryStream())
            {
                return (T)ser.ReadObject(ms);
            }
        }
    }
}
