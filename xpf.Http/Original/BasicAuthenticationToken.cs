namespace xpf.Http.Original
{
    public class BasicAuthenticationToken : AuthenticationToken
    {
        public BasicAuthenticationToken(string token) : base("basic", token)
        {
        }
    }
}