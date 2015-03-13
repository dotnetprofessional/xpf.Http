using System;

namespace xpf.Http
{
    public class RequestContentTypes : IReferenceUrl
    {
        public RequestContentTypes(Url parent, Action<IContentType> setModel)
        {
            ((IReferenceUrl)this).Url = parent;
            this.Parent = parent;
            this.SetModel = setModel;
        }

        public Url Json
        {
            get
            {
                this.SetModel(new JsonEncoder());
                return this.Parent;
            } 
        }

        public Url Xml
        {
            get
            {
                this.SetModel(new XmlEncoder());
                return this.Parent;
            }
        }
        Url Parent { get; set; }
        Action<IContentType> SetModel { get; set; }
        Url IReferenceUrl.Url { get; set; }
    }
}