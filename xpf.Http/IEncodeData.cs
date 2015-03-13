namespace xpf.Http
{
    public interface IEncodeData
    {
        string ContentEncoding { get; }

        string Encode(string data);

        string Decode(string data);
    }
}