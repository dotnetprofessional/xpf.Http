namespace xpf.Http
{
    public class StringContent : IContentType
    {
        public string ContentType
        {
            get { return "text/html"; }
        }

        public string Serialize<T>(T data)
        {
            return data as string;
        }

        public T Deserialize<T>(string data)
        {
            return (T)(data as object);
        }
    }
}