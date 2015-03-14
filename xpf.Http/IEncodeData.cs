using System.IO;
using System.Threading.Tasks;

namespace xpf.Http
{
    public interface IEncodeData
    {
        string ContentEncoding { get; }

        Task<Stream> Encode(string data);

        Task<string> Decode(Stream data);
    }
}