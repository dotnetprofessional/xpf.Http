namespace xpf.Http
{
    public static class EncodingExtensions
    {
        public static Url Base64(this RequestContentTypes contentTypes)
        {
            var parent = ((IReferenceUrl) contentTypes).Url;
            parent.Model.Encoding = new Base64Encoder();
            return parent;
        }
    }
}