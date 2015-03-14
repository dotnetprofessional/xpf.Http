using System;

namespace xpf.Http
{
    public class RequestContentTypes : IRequireNavigationContext
    {
        public RequestContentTypes(NavigationContext parent, Action<IContentType> setModel)
        {
            ((IRequireNavigationContext)this).NavigationContext = parent;
            this.Parent = parent;
            this.SetModel = setModel;
        }

        public NavigationContext Json
        {
            get
            {
                this.SetModel(new JsonContent());
                return this.Parent;
            } 
        }

        public NavigationContext Xml
        {
            get
            {
                this.SetModel(new XmlContent());
                return this.Parent;
            }
        }
        NavigationContext Parent { get; set; }
        Action<IContentType> SetModel { get; set; }
        NavigationContext IRequireNavigationContext.NavigationContext { get; set; }
    }
}