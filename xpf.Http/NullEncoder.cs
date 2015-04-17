using System;
using System.IO;
using System.Threading.Tasks;

namespace xpf.Http
{
    public class NullEncoder : IEncodeData
    {
        public string ContentEncoding
        {
            get { return "text/html"; }
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
