using System;

namespace xpf.Http
{
    public class BearerToken
    {
        public string Token { get; set; }

        public string Type { get; set; }

        public DateTime Created { get; set; }

        public DateTime Expires { get; set; }

        public bool HasExpired  {get { return this.Expires < DateTime.Now; }}
    }
}