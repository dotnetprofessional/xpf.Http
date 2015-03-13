using System;
using System.Text;

namespace xpf.Http
{
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
}