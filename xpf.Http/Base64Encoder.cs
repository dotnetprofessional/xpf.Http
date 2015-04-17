using System.IO;
using System.Threading.Tasks;

namespace xpf.Http
{
    public class Base64Encoder : IEncodeData
    {
        public string ContentEncoding
        {
            get { return "base64"; }
        }

        public async Task<Stream> Encode(string data)
        {
            var stream = new MemoryStream();
            using (var streamWriter = new StreamWriter(new MemoryStream()))
            {
                await streamWriter.WriteAsync(data);
            }

            return stream;
        }

        public async Task<string> Decode(Stream data)
        {
            return await new StreamReader(data).ReadToEndAsync();
        }
    }
}


 