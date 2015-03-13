namespace xpf.Http
{
    public class EncodingTypes : IReferenceUrl
    {
        public EncodingTypes(Url parent)
        {
            ((IReferenceUrl)this).Url = parent;
            this.Parent = parent;
        }

        
        public Url Base64
        {
            get
            {
                this.Parent.Model.Encoding = new Base64Encoder();
                return this.Parent;
            }
        }

        Url Parent { get; set; }
        Url IReferenceUrl.Url { get; set; }
    }
}