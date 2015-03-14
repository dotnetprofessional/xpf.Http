namespace xpf.Http
{
    public static class EncodingExtensions
    {
        public static NavigationContext Base64(this RequestContentTypes contentTypes)
        {
            var parent = ((IRequireNavigationContext) contentTypes).NavigationContext;
            parent.Model.Encoding = new Base64Encoder();
            return parent;
        }
    }
}