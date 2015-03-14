using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;

namespace xpf.Http
{
    public class Http
    {
        public Http()
        {
        }


        public Http(HttpMessageHandler messageHandler)
        {
            this.MessageHandler = messageHandler;
        }

        HttpMessageHandler MessageHandler { get; set; }

        public NavigationContext Navigate(string url)
        {
            return new NavigationContext(this.MessageHandler, url);
        }

    }
}

/*

 *  Add a Then method to the HttpResult that will act as a continuation. Transfer all data that makes sense into a new model such as Cookies
 *  and referrer (ie current page), user-agent, accept-encoding, accept, 

*/