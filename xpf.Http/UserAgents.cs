using xpf.Http.Original;

namespace xpf.Http
{
    public class UserAgents : IRequireNavigationContext
    {
        public UserAgents(NavigationContext parent)
        {
            ((IRequireNavigationContext)this).NavigationContext = parent;
            this.Parent = parent;
        }


        public NavigationContext IE11
        {
            get
            {
                this.Parent.Model.Headers.Add(new HttpHeader{Key = "User-Agent", Value = new []{@"Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko"}});
                return this.Parent;
            }
        }

        NavigationContext Parent { get; set; }
        NavigationContext IRequireNavigationContext.NavigationContext { get; set; }
    }
}