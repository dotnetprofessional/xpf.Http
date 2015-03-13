namespace xpf.Http
{
    public interface IContentType
    {
        string ContentType { get; }

        string Serialize<T>(T data);
        T Deserialize<T>(string data);
    }
}