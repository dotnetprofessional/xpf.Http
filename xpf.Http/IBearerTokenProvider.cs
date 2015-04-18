using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xpf.Http
{
    public interface IBearerTokenProvider
    {
        Task<BearerToken> AcquireTokenAsync(string username, string password);
    }
}
