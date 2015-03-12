namespace xpf.Http.Original
{
    public class AuthenticationToken
    {
        public AuthenticationToken(string scheme, string token)
        {
            this.Scheme = scheme;
            this.Value = token;
        }

        public string Scheme { get; protected set; }

        public string Value { get; protected set; }
    }
}