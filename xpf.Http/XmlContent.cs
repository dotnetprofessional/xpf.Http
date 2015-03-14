using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace xpf.Http
{
    public class XmlContent : IContentType
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
}