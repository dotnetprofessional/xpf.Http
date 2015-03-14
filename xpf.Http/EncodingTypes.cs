namespace xpf.Http
{
    public class EncodingTypes : IRequireNavigationContext
    {
        public EncodingTypes(NavigationContext parent)
        {
            ((IRequireNavigationContext)this).NavigationContext = parent;
            this.Parent = parent;
        }

        
        public NavigationContext Base64
        {
            get
            {
                this.Parent.Model.Encoding = new Base64Encoder();
                return this.Parent;
            }
        }

        NavigationContext Parent { get; set; }
        NavigationContext IRequireNavigationContext.NavigationContext { get; set; }
    }
}