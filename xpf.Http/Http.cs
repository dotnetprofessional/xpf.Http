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

        public Url Url(string url)
        {
            return new Url(this.MessageHandler, url);
        }

    }

    //public class CookieGrammar 
    //{
    //    HttpCookie CurrentCookie { get; set; }
    //    Url Url { get; set; }
    //    public CookieGrammar(Url url, string name, string value)
    //    {
    //        this.Url = url;
    //        this.CurrentCookie = new HttpCookie {Name = name, Value = value};
    //        this.Url.Model.Cookies.Add(this.CurrentCookie);
    //    }

    //    public Url And { get { return this.Url; } }

    //    public CookieGrammar ExpiryDate(DateTime expiry)
    //    {
    //        this.CurrentCookie.Expiry = expiry;
    //        return this;
    //    }
    //}
}

/*

 *  Add a Then method to the HttpResult that will act as a continuation. Transfer all data that makes sense into a new model such as Cookies
 *  and referrer (ie current page), user-agent, accept-encoding, accept, 

*/