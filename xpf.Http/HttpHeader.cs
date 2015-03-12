using System.Collections.Generic;

namespace xpf.Http.Original
{
    public class HttpHeader
    {
        public string Key { get; set; }

        public IEnumerable<string> Value { get; set; }
    }

    public class HttpFormValue
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}