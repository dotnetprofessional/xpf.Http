using System.Linq;
using System.Linq.Expressions;

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
                this.SetEncodingHeader(new Base64Encoder());
                return this.Parent;
            }
        }

        public NavigationContext Gzip
        {
            get
            {
                this.SetEncodingHeader(new GzipEncoder());
                return this.Parent;
            }
        }

        void SetEncodingHeader(IEncodeData encoding)
        {
            if (this.Parent.Model.Headers.Contains("Accept"))
                this.Parent.Model.Headers.Remove("Accept");

            this.Parent.WithHeader("Accept-Encoding", encoding.ContentEncoding);
            this.Parent.Model.Encoding = encoding;
        }

        NavigationContext Parent { get; set; }
        NavigationContext IRequireNavigationContext.NavigationContext { get; set; }
    }
}