using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace xpf.Http
{
    public class GzipEncoder : IEncodeData
    {
        public string ContentEncoding
        {
            get { return "gzip, deflate"; }
        }

        public async Task<Stream> Encode(string data)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);

            MemoryStream memory = new MemoryStream();
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(dataBytes, 0, dataBytes.Length);
                }
                return memory;
            }
        }

        public async Task<string> Decode(Stream data)
        {
            using (GZipStream stream = new GZipStream(data, CompressionMode.Decompress))
            {
                var reader = new StreamReader(stream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
    }
}